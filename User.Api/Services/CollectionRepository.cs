using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;

namespace User.Api.Services
{
    public class CollectionRepository<T> : ICollectionRepository<T> where T : class
    {
        protected CollectionReference Collection { get; private set; }
        protected FirestoreDb Firestore { get; private set; }

        public CollectionRepository(IConfiguration configuration)
        {
            Firestore = FirestoreDb.Create(configuration.GetValue<string>("GCPProjectName"));
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

        public async Task<IEnumerable<T>> GetByFieldAsync(string fieldName, string value)
        {
            var snapshot = await Collection.WhereEqualTo(fieldName, value)
                                     .GetSnapshotAsync();

            var result = new List<T>();
            foreach (var doc in snapshot.Documents)
            {
                if (!doc.Exists) { continue; }
                result.Add(doc.ConvertTo<T>());
            }
            return result;
        }

        public virtual async Task InsertAsync(string documentId, T value)
        {
            var doc = Collection.Document(documentId);
            await doc.SetAsync(value);
        }

        public virtual async Task InsertAsync(T value)
        {
            var doc = Collection.Document();
            await doc.SetAsync(value);
        }

        public async Task UpdateAsync(string documentId, T value)
        {
            var doc = Collection.Document(documentId);
            await doc.SetAsync(value);
        }
    }
}
