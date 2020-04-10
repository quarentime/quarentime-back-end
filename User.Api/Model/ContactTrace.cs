using System.Collections.Generic;
using User.Api.Extensions;

namespace User.Api.Model
{
    public class ContactTrace
    {
        public string Name { get; set; }
        public string Initials
        {
            get
            {
               return Name.Initials();
            }
        }
        public RiskGroup FinalStatus { get; set; }
        public IEnumerable<ContactTrace> Contacts { get; set; } = new List<ContactTrace>();
        public string ColorHex { get; set; }
        public bool IsDirectContact { get; set; }
        public bool Pending { get; set; }

        
    }
}
