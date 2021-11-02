using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Services
{
    public interface IUserService
    {
        Task<IdentityUser> GetUserByIdAsync(string id);
    }
}
