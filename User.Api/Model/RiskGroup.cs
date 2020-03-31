using Google.Cloud.Firestore;

namespace User.Api.Model
{
    [FirestoreData(ConverterType = typeof(FirestoreEnumNameConverter<RiskGroup>))]
    public enum RiskGroup
    {
        Healthy,
        HealtySocialDistancing,
        LowProbabilitySuspected,
        HighProbabilitySuspected,
        FluLike,
        Positive,
        Recovered
    }
}
