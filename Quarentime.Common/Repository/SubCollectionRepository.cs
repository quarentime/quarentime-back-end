using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Quarentime.Common.Attributes;

namespace Quarentime.Common.Repository
{
    public class SubCollectionRepository<T> : ISubCollectionRepository<T> where T : class
    {
        protected FirestoreDb Firestore { get; private set; }
        protected string Path { get; private set; }
        
        public SubCollectionRepository(IConfiguration configuration)
        {
            Firestore = FirestoreDb.Create(configuration.GetSection("GCPProjectName").Value);

            var attributes = typeof(T).GetCustomAttributes(false);
            var attr = (EntityPathAttribute)attributes.First(a => a is EntityPathAttribute);
            Path = attr.Path;
        }

        protected CollectionReference Collection(string rootId)
        {
            return Firestore.Collection(string.Format(Path, rootId));
        }

        public async Task<bool> DeleteAsync(string rootId, string documentId)
        {
            var doc = Collection(rootId).Document(documentId);
            var snapshot = await doc.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return false;
            }

            await doc.DeleteAsync();
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string rootId)
        {
            var snapshot = await Collection(rootId).GetSnapshotAsync();
            var result = new List<T>();

            foreach (var doc in snapshot.Documents)
            {
                if (!doc.Exists) { continue; }
                result.Add(doc.ConvertTo<T>());
            }
            return result;
        }

        public async Task<T> GetByIdAsync(string rootId, string documentId)
        {
            var doc = Collection(rootId).Document(documentId);
            var snapshot = await doc.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<T>();
            }
            return null;
        }

        public async Task<IEnumerable<T>> GetByFieldAsync(string rootId, string fieldName, string value)
        {
            var snapshot = await Collection(rootId).WhereEqualTo(fieldName, value)
                                     .GetSnapshotAsync();

            var result = new List<T>();
            foreach (var doc in snapshot.Documents)
            {
                if (!doc.Exists) { continue; }
                result.Add(doc.ConvertTo<T>());
            }
            return result;
        }

        public virtual async Task InsertAsync(string rootId, T value)
        {
            var doc = Collection(rootId).Document();
            await doc.SetAsync(value);
        }

        public virtual async Task InsertAsync(string rootId, string documentId, T value)
        {
            var doc = Collection(rootId).Document(documentId);
            await doc.SetAsync(value);
        }

        public async Task UpdateAsync(string rootId, string documentId, T value)
        {
            var doc = Collection(rootId).Document(documentId);
            await doc.SetAsync(value);
        }
    }
}
