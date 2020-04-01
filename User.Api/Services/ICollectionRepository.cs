using System.Collections.Generic;
using System.Threading.Tasks;

namespace User.Api.Services
{
    public interface ICollectionRepository<T> where T : class
    {
        Task<bool> DeleteAsync(string documentId);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string documentId);
        Task<IEnumerable<T>> GetByFieldAsync(string fieldName, string value);
        Task InsertAsync(string documentId, T value);
        Task InsertAsync(T value);
        Task UpdateAsync(string documentId, T value);
    }
}
