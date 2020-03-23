using Google.Cloud.Firestore;

namespace User.Api.Model
{
    [FirestoreData]
    public class SurveyIntake
    {
        [FirestoreProperty] public bool HasRecentTravel { get; set; }
        [FirestoreProperty] public bool HasCloseContact { get; set; }
        [FirestoreProperty] public bool HasSymptoms { get; set; }
        [FirestoreProperty] public bool HasRecovered { get; set; }
    }
}
