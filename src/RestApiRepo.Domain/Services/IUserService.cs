using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Services
{
    public interface IUserService
    {
        Task<IdentityUser> GetUserByIdAsync(string id);
    }
}
