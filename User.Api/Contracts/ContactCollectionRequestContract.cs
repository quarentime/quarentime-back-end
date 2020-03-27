using System.Collections.Generic;
using User.Api.Model;

namespace User.Api.Contracts
{
    public class ContactCollectionRequestContract
    {
        public IEnumerable<Contact> Contacts { get; set; }
    }
}
