using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using User.Api.Exceptions;
using User.Api.Model;
using Quarentime.Common.Repository;
using Quarentime.Common.Services;
using Quarentime.Common.Contracts;
using Quarentime.Common.Models;
using Microsoft.Extensions.Logging;
using User.Api.Contracts;
using Quarentime.Common.Helpers;

namespace User.Api.Services
{
    public class ContactsService : IContactsService
    {
        private readonly ILogger<ContactsService> _logger;
        private readonly ISubCollectionRepository<Contact> _contactRepository;
        private readonly ICollectionRepository<Invite> _inviteRepository;
        private readonly IUserService _userService;
        private readonly ICloudTaskService _cloudTaskService;

        public ContactsService(ISubCollectionRepository<Contact> contactRepository,
                                ICollectionRepository<Invite> inviteRepository,
                                IUserService userService,
                                ICloudTaskService cloudTaskService,
                                ILogger<ContactsService> logger)
        {
            _logger = logger;
            _cloudTaskService = cloudTaskService;
            _contactRepository = contactRepository;
            _inviteRepository = inviteRepository;
            _userService = userService;
        }

        public async Task<IEnumerable<AddContactResult>> InsertManyAsync(string userId, IEnumerable<BasicContactInfo> contacts)
        {
            var result = new List<AddContactResult>();

            var currentUser = await _userService.GetPersonalInformationAsync(userId);

            foreach (var contact in contacts)
            {
                var alreadyExists = (await _contactRepository
                        .GetByFieldAsync(rootId: userId, nameof(Contact.PhoneNumber), contact.PhoneNumber)).Any();

                if (alreadyExists)
                {
                    result.Add(new AddContactResult(contact.PhoneNumber, "already_exists"));
                    continue;
                }

                // Check if we already have an invite from that user
                var invitation = (await _inviteRepository
                                    .GetByFieldAsync(nameof(Invite.FromUserPhoneNumber), contact.PhoneNumber))
                                    .FirstOrDefault();

                if (invitation != null)
                {
                    await AcceptInviteAsync(userId, invitation.InviteId);
                    result.Add(new AddContactResult(contact.PhoneNumber, "invitation_accepted"));
                    continue;
                }

                var message = new MessageContract()
                {
                    //Please make a better message. We can contact PR for all of this messages
                    Message = $"Hello, {contact.Name}! {currentUser.DisplayName} has invited you to their contract trace on Quarentime. If you don't have the app yet, you can download it from https://appdistribution.firebase.dev/i/UbX9ViLM",
                    Title = $"Contact trace invitation",
                    UserId = currentUser.UserId,
                    UserPhoneNumber = contact.PhoneNumber
                };
                try
                {
                    await _cloudTaskService.SendMessage(message, NotificationType.SmsNotifications);
                    await _inviteRepository.InsertAsync($"{userId}{contact.PhoneNumber}", new Invite
                    {
                        FromUserId = userId,
                        FromUserName = currentUser.DisplayName,
                        FromUserPhoneNumber = currentUser.PhoneNumber,
                        PhoneNumber = contact.PhoneNumber,
                        Name = contact.Name,
                        DateAdded = DateTime.UtcNow,
                        Pending = true,
                        IsDirectContact = contact.IsDirectContact
                    });
                    result.Add(new AddContactResult(contact.PhoneNumber, "invitation_sent"));

                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Failed to invite contact; ExceptionMessage: {e.Message}", contact);
                }
            }
            return result;
        }

        public async Task<IEnumerable<Invite>> GetPendingInvitesAsync(string userId)
        {
            var user = await _userService.GetPersonalInformationAsync(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }

            return await _inviteRepository.GetByFieldAsync(nameof(Invite.PhoneNumber), user.PhoneNumber);
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync(string userId, BaseFilterContract request)
        {
            // My contacts
            var contacts = (await _contactRepository.GetAllAsync(rootId: userId, request?.ToDictionary())).ToList();

            // Invites that I sent
            var invites = await _inviteRepository.GetByFieldAsync(nameof(Invite.FromUserId), userId);

            contacts.AddRange(invites.Select(i => new Contact
            {
                Name = i.Name,
                PhoneNumber = i.PhoneNumber,
                Pending = i.Pending,
                DateAdded = i.DateAdded,
                Status = RiskGroup.Pending,
                IsDirectContact = i.IsDirectContact
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
                IsDirectContact = invite.IsDirectContact,
                Status = await _userService.GetRiskGroupAsync(invite.FromUserId)
            });

            // Adding contact to the other user
            await _contactRepository.InsertAsync(rootId: invite.FromUserId, documentId: userId, value: new Contact
            {
                Name = invite.Name,
                PhoneNumber = invite.PhoneNumber,
                UserId = userId,
                DateAdded = DateTime.UtcNow,
                IsDirectContact = invite.IsDirectContact,
                Status = await _userService.GetRiskGroupAsync(userId)
            });

            invite.Pending = false;
            await _inviteRepository.DeleteAsync(inviteId);
        }

        public async Task RejectInvite(string inviteId)
        {
            await _inviteRepository.DeleteAsync(inviteId);
        }

        public async Task<ContactTrace> GetContactTrace(string userId, GetContactTraceContract request)
        {
            var trace = await _userService.GetUserTraceData(userId);
            if (request != null && !request.DirectOnly)
                request = null;

            var contacts = await GetAllContactsAsync(userId, request);

            trace.Contacts = contacts.Select(c => new ContactTrace
            {
                Name = c.Name,
                FinalStatus = c.Status,
                ColorHex = RiskGroupToHexMapper.HexMapper[c.Status],
                Pending = c.Pending,
                IsDirectContact = c.IsDirectContact
            });

            return trace;
        }
    }
}
