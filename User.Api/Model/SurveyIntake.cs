using Google.Cloud.Firestore;
using System.Collections.Generic;

namespace User.Api.Model
{
    [FirestoreData]
    public class SurveyIntake
    {
        [FirestoreProperty] public bool HasRecentTravelLast14Days { get; set; }
        [FirestoreProperty] public bool HasRecentTravelBeforeSymptoms { get; set; }
        [FirestoreProperty] public bool HasCloseContact { get; set; }
        [FirestoreProperty] public bool HasSymptoms { get; set; }
        [FirestoreProperty] public bool HasRecovered { get; set; }
        [FirestoreProperty] public bool IsTestedPositive { get; set; }
        [FirestoreProperty] public List<Symptom> Symptoms { get; set; }
    }
}
