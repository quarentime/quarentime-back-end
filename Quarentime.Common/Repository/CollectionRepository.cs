using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;

namespace Quarentime.Common.Repository
{
    public class CollectionRepository<T>: ICollectionRepository<T> where T:class
    {
        protected CollectionReference Collection { get; private set; }
        protected FirestoreDb Firestore { get; private set; }
        
        public CollectionRepository(IConfiguration configuration)
        {
            Firestore = FirestoreDb.Create(configuration.GetSection("GCPProjectName").Value);
            Collection = Firestore.Collection(typeof(T).Name);
        }

        public async Task<bool> DeleteAsync(string documentId)
        {
            var doc = Collection.Document(documentId);
            var snapshot = await doc.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return false;
            }

            await doc.DeleteAsync();
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var snapshot = await Collection.GetSnapshotAsync();
            var result = new List<T>();

            foreach (var doc in snapshot.Documents)
            {
                if (!doc.Exists) { continue; }
                result.Add(doc.ConvertTo<T>());
            }
            return result;
        }

        public async Task<T> GetByIdAsync(string documentId)
        {
            var doc = Collection.Document(documentId);
            var snapshot = await doc.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<T>();
            }
            return null;
        }

        public virtual async Task InsertAsync(string documentId, T value)
        {
            var doc = Collection.Document(documentId);
            await doc.SetAsync(value);
        }

        public async Task UpdateAsync(string documentId, T value)
        {
            var doc = Collection.Document(documentId);
            await doc.SetAsync(value);
        }
    }
}
