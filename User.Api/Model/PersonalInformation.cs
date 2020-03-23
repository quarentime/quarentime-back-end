using Google.Cloud.Firestore;

namespace User.Api.Model
{
    [FirestoreData]
    public class PersonalInformation
    {
        [FirestoreProperty] public string Email { get; set; }
        [FirestoreProperty] public string Name { get; set; }
        [FirestoreProperty] public string Surname { get; set; }
        [FirestoreProperty] public int Age { get; set; }
        [FirestoreProperty] public string PhoneNumber { get; set; }
    }
}
