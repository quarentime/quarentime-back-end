using Google.Cloud.Firestore;

namespace Quarentime.Common.Services
{
    [FirestoreData]
    public class ConfigurationSetting
    {
        [FirestoreProperty]public string Value { get; set; }
    }
}
