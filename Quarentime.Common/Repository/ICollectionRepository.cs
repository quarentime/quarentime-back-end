using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quarentime.Common.Repository
{
    public interface ICollectionRepository<T> where T : class
    {
        Task<bool> DeleteAsync(string documentId);
        Task<IEnumerable<T>> GetAllAsync(IDictionary<string, string> filterParams = null);
        Task<T> GetByIdAsync(string documentId);
        Task<IEnumerable<T>> GetByFieldAsync(string fieldName, string value);
        Task InsertAsync(string documentId, T value);
        Task InsertAsync(T value);
        Task UpdateAsync(string documentId, T value);
    }
}
