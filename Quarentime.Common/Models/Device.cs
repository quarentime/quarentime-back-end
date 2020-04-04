using Google.Cloud.Firestore;
using Quarentime.Common.Attributes;

namespace Quarentime.Common.Models
{
    [FirestoreData]
    [EntityPath("User/{0}/Devices")]
    public class Device
    {
        public string Token { get; set; }

        public Device(string token) => Token = token;
    }
}
