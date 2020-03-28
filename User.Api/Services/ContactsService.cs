using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using User.Api.Exceptions;
using User.Api.Model;

namespace User.Api.Services
{
    public class ContactsService : IContactsService
    {
        private readonly ISubCollectionRepository<Contact> _contactRepository;
        private readonly ICollectionRepository<Invite> _inviteRepository;
        private readonly IUserService _userService;

        public ContactsService(ISubCollectionRepository<Contact> contactRepository,
                                ICollectionRepository<Invite> inviteRepository,
                                IUserService userService)
        {
            _contactRepository = contactRepository;
            _inviteRepository = inviteRepository;
            _userService = userService;
        }

        public async Task InsertManyAsync(string userId, IEnumerable<Contact> contacts)
        {
            var currentUser = await _userService.GetPersonalInformationAsync(userId);

            foreach (var contact in contacts)
            {
                var alreadyExists = (await _contactRepository
                        .GetByFieldAsync(rootId: userId, nameof(Contact.PhoneNumber), contact.PhoneNumber)).Any();

                if (alreadyExists)
                {
                    continue;
                }

                await _inviteRepository.InsertAsync($"{userId}{contact.PhoneNumber}", new Invite
                {
                    FromUserId = userId,
                    FromUserName = currentUser.Name,
                    FromUserPhoneNumber = currentUser.PhoneNumber,
                    PhoneNumber = contact.PhoneNumber,
                    Name = contact.Name,
                    Pending = true
                });
            }
        }

        public async Task<IEnumerable<Invite>> GetPendingInvitesAsync(string userId)
        {
            var user = await _userService.GetPersonalInformationAsync(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }

            return await _inviteRepository.GetByFieldAsync(nameof(Invite.PhoneNumber),user.PhoneNumber);
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync(string userId)
        {
            // My contacts
            var contacts =  (await _contactRepository.GetAllAsync(rootId: userId)).ToList();

            // Invites that I sent
            var invites = await _inviteRepository.GetByFieldAsync(nameof(Invite.FromUserId), userId);

            contacts.AddRange(invites.Select(i => new Contact
            {
                Name = i.Name, 
                PhoneNumber = i.PhoneNumber, 
                Pending = i.Pending
            }));

            return contacts;
        }

        public async Task AcceptInviteAsync(string userId, string inviteId)
        {
            var invite = await _inviteRepository.GetByIdAsync(inviteId);
            if (invite == null)
            {
                throw new NotFoundException();
            }

            // Adding contact to current user
            await _contactRepository.InsertAsync(rootId: userId, documentId: invite.FromUserId, value: new Contact
            {
                Name = invite.FromUserName, 
                PhoneNumber = invite.FromUserPhoneNumber, 
                UserId = invite.FromUserId, 
                DateAdded = DateTime.UtcNow,
                Status = await _userService.GetRiskGroupAsync(invite.FromUserId)
            });

            // Adding contact to the other user
             await _contactRepository.InsertAsync(rootId: invite.FromUserId, documentId: userId, value: new Contact
            {
                Name = invite.Name, 
                PhoneNumber = invite.PhoneNumber, 
                UserId = userId, 
                DateAdded = DateTime.UtcNow,
                Status = await _userService.GetRiskGroupAsync(userId)
            });

            invite.Pending = false;
            await _inviteRepository.DeleteAsync(inviteId);
        }

        public async Task RejectInvite(string inviteId)
        {
            await _inviteRepository.DeleteAsync(inviteId);
        }
    }
}
