using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace User.Api.Model
{
    public class ContactTrace
    {
        public string Name { get; set; }
        public string Initials
        {
            get
            {
                Regex initials = new Regex(@"(\b[a-zA-Z])[a-zA-Z]* ?");
                return initials.Replace(Name, "$1");
            }
        }
        public RiskGroup FinalStatus { get; set; }
        public IEnumerable<ContactTrace> Contacts { get; set; } = new List<ContactTrace>();
        public string ColorHex { get; set; }
        public bool Pending { get; set; }
    }
}
