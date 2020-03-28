using System.Collections.Generic;
using System.Threading.Tasks;
using User.Api.Model;

namespace User.Api.Services
{
    public interface IContactsService
    {
        Task InsertManyAsync(string userId, IEnumerable<Contact> contacts);
        Task<IEnumerable<Invite>> GetPendingInvitesAsync(string userId);
        Task<IEnumerable<Contact>> GetAllContactsAsync(string userId);
        Task AcceptInviteAsync(string userId, string inviteId);
        Task RejectInvite(string inviteId);
    }
}
