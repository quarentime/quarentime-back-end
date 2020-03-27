using Google.Cloud.Firestore;

namespace User.Api.Model
{
    [FirestoreData]
    public class PhoneVerificationCode
    {
        [FirestoreProperty] public string Code { get; set; }

    }
}

