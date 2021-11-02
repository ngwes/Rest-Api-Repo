using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Services
{
    public interface ICosmosDbService<T>
    {
        Task<IEnumerable<T>> GetMultipleAsync(string query);
        Task<T> GetAsync(string id);
        Task<bool> AddAsync(T item, string itemId);
        Task<bool> UpdateAsync(string id, T item);
        Task<bool> DeleteAsync(string id);
    }
}
