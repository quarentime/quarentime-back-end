using Google.Cloud.Firestore;

namespace User.Api.Model
{
    [FirestoreData]
    public class Invite
    {
        [FirestoreDocumentId] public string InviteId { get; set; }
        [FirestoreProperty] public string FromUserId { get; set; }
        [FirestoreProperty] public string Name { get; set; }
        [FirestoreProperty] public string PhoneNumber { get; set; }
        [FirestoreProperty] public bool Pending { get; set; }
    }
}