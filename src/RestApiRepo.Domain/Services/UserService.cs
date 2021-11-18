using Microsoft.AspNetCore.Identity;
using RestApiRepo.Domain.Repositories;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Services
{
    public class UserService : IUserService
    {
        //private readonly DataContext _context;

        //public UserService(DataContext context)
        //{
        //    _context = context;
        //}
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IdentityUser> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
    }
}
