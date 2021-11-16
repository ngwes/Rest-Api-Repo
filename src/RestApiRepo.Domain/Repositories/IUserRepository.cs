using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Repositories
{
    public interface IUserRepository : IRepository
    {
        public Task<IEnumerable<IdentityUser>> GetAllUsersAsync(
           Expression<Func<IdentityUser, bool>> filter = null,
           Func<IQueryable<IdentityUser>, IOrderedQueryable<IdentityUser>> orderBy = null,
           string includeProperties = "",
           int skip = 0,
            int take = 0);
        public Task<IdentityUser> GetUserByIdAsync(string id);
        public void InsertUser(IdentityUser entity);
        public Task DeleteUserByIdAsync(string id);
        public void DeleteUser(IdentityUser entityToDelete);
        public void UpdateUser(IdentityUser entityToUpdate);
    }
}
