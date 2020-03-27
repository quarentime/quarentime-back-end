using System;
using Google.Cloud.Firestore;
using User.Api.Attributes;

namespace User.Api.Model
{
    [FirestoreData]
    [EntityPath("User/{0}/Contacts")]
    public class Contact
    {
        [FirestoreProperty] public string Name { get; set; }
        [FirestoreProperty] public string PhoneNumber { get; set; }
        [FirestoreProperty] public string UserId { get; set; }
        [FirestoreProperty] public DateTime DateAdded { get; set; }
        [FirestoreProperty] public RiskGroup Status { get; set; }

    }
}
