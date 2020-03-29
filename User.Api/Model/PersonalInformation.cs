using Google.Cloud.Firestore;

namespace User.Api.Model
{
    [FirestoreData]
    public class PersonalInformation
    {
        [FirestoreDocumentId] public string UserId { get; set; }
        [FirestoreProperty] public string Email { get; set; }
        [FirestoreProperty] public string Name { get; set; }
        [FirestoreProperty] public string Surname { get; set; }
        [FirestoreProperty] public int Age { get; set; }
        [FirestoreProperty] public string PhoneNumber { get; set; }
        [FirestoreProperty] public bool Verified { get; set; }

        public string DisplayName
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }
    }
}
