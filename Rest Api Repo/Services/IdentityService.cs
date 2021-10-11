using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rest_Api_Repo.Data;
using Rest_Api_Repo.Domain;
using Rest_Api_Repo.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _dataContext;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, DataContext dataContext)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _dataContext = dataContext;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user is null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            var userHasValidEmail = await _userManager.CheckPasswordAsync(user, password);

            if(!userHasValidEmail)
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

            if(expiryDateUtc > DateTime.UtcNow)
                return new AuthenticationResult 
                { Errors = new[] { "This token has expired yet" } };

            var storedRefreshToken = await _dataContext
                .RefreshTokens
                .SingleOrDefaultAsync(x => x.Token.Equals(refreshToken));

            if(storedRefreshToken is null)
                return new AuthenticationResult
                { Errors = new[] { "This refresh token doesn't exist" } };

            if(DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return new AuthenticationResult
                { Errors = new[] { "This refresh token has expired" } };

            if(storedRefreshToken.Invalidated)
                return new AuthenticationResult
                { Errors = new[] { "This refresh token has been invalidated" } };

            if(storedRefreshToken.Used)
                return new AuthenticationResult
                { Errors = new[] { "This refresh token has been used" } };

            var jti = validateToken.Claims
               .Single(c => c.Type.Equals(JwtRegisteredClaimNames.Jti)).Value;

            if(storedRefreshToken.JwtId != jti)
                return new AuthenticationResult
                { Errors = new[] { "This refresh token doesn't match this JWT" } };

            storedRefreshToken.Used = true;
            _dataContext.RefreshTokens.Update(storedRefreshToken);
            await _dataContext.SaveChangesAsync();

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

            //password here will go through Microsoft password hasher
            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            return await GenerateAuthenticationResultForUserAsync(newUser);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", user.Id),
                }),
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

            await _dataContext.RefreshTokens.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();

            return new AuthenticationResult
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token,
                Success = true
            };
        }
    }
}
