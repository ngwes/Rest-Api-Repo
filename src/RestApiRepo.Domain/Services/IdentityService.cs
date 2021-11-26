using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RestApiRepo.Domain.Configurations;
using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.InProcessNotifications;
using RestApiRepo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly IEmailService _emailService;
        private readonly AuthenticationSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMediator _mediator;

        public IdentityService(UserManager<IdentityUser> userManager,
            AuthenticationSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters,
             IRefreshTokenRepository refreshTokenRepository/*, IEmailService emailService*/, IMediator mediator)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _refreshTokenRepository = refreshTokenRepository;
            _mediator = mediator;
            //_emailService = emailService;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            var userHasValidEmail = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidEmail)
                return new AuthenticationResult
                {
                    Errors = new[] { "User/Password combination is wrong" }
                };

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validateToken = GetClaimsPrincipalFromToken(token);

            if (validateToken is null)
                return new AuthenticationResult { Errors = new[] { "Invalid token" } };

            var expiryDateUnix = long.Parse(validateToken.Claims
                .Single(c => c.Type.Equals(JwtRegisteredClaimNames.Exp))
                .Value);

            var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateUtc > DateTime.UtcNow)
                return new AuthenticationResult
                { Errors = new[] { "This token has expired yet" } };

            var storedRefreshToken = await _refreshTokenRepository.GetRefreshTokenByTokenIdAsync(refreshToken);

            if (storedRefreshToken is null)
                return new AuthenticationResult
                { Errors = new[] { "This refresh token doesn't exist" } };

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return new AuthenticationResult
                { Errors = new[] { "This refresh token has expired" } };

            if (storedRefreshToken.Invalidated)
                return new AuthenticationResult
                { Errors = new[] { "This refresh token has been invalidated" } };

            if (storedRefreshToken.Used)
                return new AuthenticationResult
                { Errors = new[] { "This refresh token has been used" } };

            var jti = validateToken.Claims
               .Single(c => c.Type.Equals(JwtRegisteredClaimNames.Jti)).Value;

            if (storedRefreshToken.JwtId != jti)
                return new AuthenticationResult
                { Errors = new[] { "This refresh token doesn't match this JWT" } };

            storedRefreshToken.Used = true;
            _refreshTokenRepository.UpdateToken(storedRefreshToken);
            await _refreshTokenRepository.UnitOfWork.SaveEntitiesAsync();
            
            var user = await _userManager
                .FindByIdAsync(validateToken.Claims
                .Single(c => c.Type.Equals("id")).Value);

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token,
                    _tokenValidationParameters,
                    out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                    return null;
                else return principal;
            }
            catch (Exception)
            {

                return null;
            }
        }
        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken token)
        {
            return (token is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCulture);
        }
        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (!(existingUser is null))
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email already exists" }
                };
            }

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }
            //await _userManager.AddClaimAsync(newUser, new Claim("tag.view", "true"));
            await _userManager.AddToRoleAsync(newUser, "Poster");
            await _mediator.Publish(new UserRegisteredEvent { 
                UserEmail = newUser.Email,
                UserId = newUser.Id
            });
           
            return await GenerateAuthenticationResultForUserAsync(newUser);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", user.Id),
                };

            //Add user claims to token
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            //Add user roles to token
            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims: claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString()
            };

             _refreshTokenRepository.InsertToken(refreshToken);
            await _refreshTokenRepository.UnitOfWork.SaveEntitiesAsync();

            return new AuthenticationResult
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token,
                Success = true
            };
        }
    }
}
