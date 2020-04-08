using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace User.Api.Model
{
    public class BasicContactInfo
    {
        [Required(ErrorMessage = "name_required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "phone_number_required"), RegularExpression(@"^\+[1-9]\d{1,14}$", ErrorMessage = "phone_number_invalid")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "is_direct_contact_required")] 
        public bool IsDirectContact { get; set; }
    }
}
