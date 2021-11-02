using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rest_Api_Repo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<IdentityUser> GetUserByIdAsync(string id)
        {
            return await _context.Users.SingleAsync(x => x.Id.Equals(id));
        }
    }
}
