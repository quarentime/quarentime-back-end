using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

namespace User.Api.Model
{
    [FirestoreData]
    public class PersonalInformation
    {
        [FirestoreDocumentId] public string UserId { get; set; }

        [Required(ErrorMessage = "email_required")]
        [FirestoreProperty] public string Email { get; set; }

        [Required(ErrorMessage = "name_required")]
        [FirestoreProperty] public string Name { get; set; }

        [Required(ErrorMessage = "surname_required")]
        [FirestoreProperty] public string Surname { get; set; }

        [Required(ErrorMessage = "age_required")]
        [Range(1, 999, ErrorMessage = "age_invalid")]
        [FirestoreProperty] public int Age { get; set; }

        [Required(ErrorMessage = "phone_number_required")]
        [RegularExpression(@"^\+[1-9]\d{1,14}$", ErrorMessage = "phone_number_invalid")]
        [FirestoreProperty] public string PhoneNumber { get; set; }
        [FirestoreProperty] public bool Verified { get; set; }

        public string DisplayName
        {
            get
            {
                return string.Join(' ', Name, Surname).Trim();
            }
        }
    }
}
