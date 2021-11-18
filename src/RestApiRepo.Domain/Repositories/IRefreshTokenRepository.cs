using RestApiRepo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Repositories
{
    public interface IRefreshTokenRepository : IRepository
    {
         public Task<IEnumerable<RefreshToken>> GetAllTokenAsync(
           Expression<Func<RefreshToken, bool>> filter = null,
           Func<IQueryable<RefreshToken>, IOrderedQueryable<RefreshToken>> orderBy = null,
           string includeProperties = "",
           int skip = 0,
            int take = 0);
        public Task<RefreshToken> GetRefreshTokenByTokenIdAsync(string tokenId);
        public void InsertToken(RefreshToken entity);
        public Task DeleteRefreshTokenByTokenIdAsync(string tokenId);
        public void DeleteToken(RefreshToken entityToDelete);
        public void UpdateToken(RefreshToken entityToUpdate);
    }
}
