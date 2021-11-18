using Microsoft.AspNetCore.Identity;
 
using RestApiRepo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<IdentityUser>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
           
        }
        public async Task DeleteUserByIdAsync(string id)
        {
            await base.DeleteByIdAsync(id);
        }

        public void DeleteUser(IdentityUser entityToDelete)
        {
            base.Delete(entityToDelete);
        }


        public async Task<IEnumerable<IdentityUser>> GetAllUsersAsync(Expression<Func<IdentityUser, bool>> filter = null, Func<IQueryable<IdentityUser>, IOrderedQueryable<IdentityUser>> orderBy = null, string includeProperties = "", int skip = 0, int take = 0)
        {
            return await base.GetAsync(filter, orderBy, includeProperties, skip, take);
        }

        public async Task<IdentityUser> GetUserByIdAsync(string id)
        {
            return await base.GetByIdAsync(id);
        }


        public void InsertUser(IdentityUser entity)
        {
            base.Insert(entity);
        }

        public void UpdateUser(IdentityUser entityToUpdate)
        {
            base.Update(entityToUpdate);
        }
    }
}
