namespace User.Api.Model
{
    public class AddContactResult
    {
        public string PhoneNumber { get; set; }
        public string Result { get; set; }
        public AddContactResult(string phone, string result)
        {
            PhoneNumber = phone;
            Result = result;
        }
    }
}
