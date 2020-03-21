using System;

namespace User.Api.Model
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int DaysInQuarantine { get; set; }
        public int DaysInPostQuarantine { get; set; }
    }
}
