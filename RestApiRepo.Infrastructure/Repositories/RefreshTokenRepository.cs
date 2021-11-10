using Rest_Api_Repo.Domain.Entities;
using Rest_Api_Repo.Infrastructure;
using RestApiRepo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DataContext context) : base(context)
        {
        }

        public void DeleteToken(RefreshToken entityToDelete)
        {
            base.Delete(entityToDelete);
        }
        
        public async Task DeleteRefreshTokenByTokenIdAsync(string tokenId)
        {
            await base.DeleteByIdAsync(tokenId);
            
        }

        public async Task<IEnumerable<RefreshToken>> GetAllTokenAsync(Expression<Func<RefreshToken, bool>> filter = null, Func<IQueryable<RefreshToken>, IOrderedQueryable<RefreshToken>> orderBy = null, string includeProperties = "", int skip = 0, int take = 0)
        {
            return await base.GetAsync(filter, orderBy, includeProperties, skip, take);
        }

        public async Task<RefreshToken> GetRefreshTokenByTokenIdAsync(string tokenId)
        {
            return await base.GetByIdAsync(tokenId);
        }

        public void InsertToken(RefreshToken entity)
        {
            base.Insert(entity);
        }

        public void UpdateToken(RefreshToken entityToUpdate)
        {
            base.Update(entityToUpdate);
        }
    }
}
