using Google.Cloud.Firestore;

namespace User.Api.Configuration
{
    [FirestoreData]
    public class ConfigurationSetting
    {
        [FirestoreProperty]public string Value { get; set; }
    }
}
