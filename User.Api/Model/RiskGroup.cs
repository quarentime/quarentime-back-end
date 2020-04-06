using Google.Cloud.Firestore;

namespace User.Api.Model
{
    [FirestoreData(ConverterType = typeof(FirestoreEnumNameConverter<RiskGroup>))]
    public enum RiskGroup
    {
        Healthy = 0,
        HealtySocialDistancing = 1,
        LowProbabilitySuspected = 2,
        HighProbabilitySuspected = 3,
        FluLike = 4,
        Positive = 5,
        Recovered = 6,
        Pending = 7
    }
}
