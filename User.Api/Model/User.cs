using Google.Cloud.Firestore;

namespace User.Api.Model
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty] public RiskGroup Status { get; set; }
        [FirestoreProperty] public RiskGroup FinalStatus { get; set; }
    }
}
