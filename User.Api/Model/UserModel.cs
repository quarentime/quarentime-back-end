using System;

namespace User.Api.Model
{
    public class UserModel
    {
        public string Id { get; set; }
        public PersonalInformation PersonalInformation { get; set; }
        public RiskFactors RiskFactors { get; set; }
        public QuarantineInfo QuarantineInfo { get; set; }
    }
}
