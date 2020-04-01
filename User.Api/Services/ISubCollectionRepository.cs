using System.Collections.Generic;
using System.Threading.Tasks;

namespace User.Api.Services
{
    public interface ISubCollectionRepository<T> where T : class
    {
        Task<bool> DeleteAsync(string rootId, string documentId);
        Task<IEnumerable<T>> GetAllAsync(string rootId);
        Task<T> GetByIdAsync(string rootId, string documentId);
        Task<IEnumerable<T>> GetByFieldAsync(string rootId, string fieldName, string value);
        Task InsertAsync(string rootId, T value);
        Task InsertAsync(string rootId, string documentId, T value);
        Task UpdateAsync(string rootId, string documentId, T value);
    }
}
