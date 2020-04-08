using System;
using Google.Cloud.Firestore;
using Quarentime.Common.Attributes;

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
        [FirestoreProperty] public bool IsDirectContact { get; set; }
        public bool Pending { get; set; }

    }
}
