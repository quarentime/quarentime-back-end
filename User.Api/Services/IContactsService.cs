using Quarentime.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using User.Api.Contracts;
using User.Api.Model;

namespace User.Api.Services
{
    public interface IContactsService
    {
        Task<IEnumerable<AddContactResult>> InsertManyAsync(string userId, IEnumerable<BasicContactInfo> contacts);
        Task<IEnumerable<Invite>> GetPendingInvitesAsync(string userId);
        Task<IEnumerable<Contact>> GetAllContactsAsync(string userId, BaseFilterContract request);
        Task AcceptInviteAsync(string userId, string inviteId);
        Task RejectInvite(string inviteId);
        Task<ContactTrace> GetContactTrace(string userId, GetContactTraceContract request);
    }
}
