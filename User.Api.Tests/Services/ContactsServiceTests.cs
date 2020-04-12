using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Quarentime.Common.Repository;
using Quarentime.Common.Services;
using User.Api.Model;
using User.Api.Services;
using Xunit;

namespace User.Api.Tests.Services
{
    public class ContactsServiceTests : IDisposable
    {
        private readonly Mock<ISubCollectionRepository<Contact>> _contactRepository;
        private readonly Mock<ICollectionRepository<Invite>> _inviteRepository;
        private readonly Mock<IUserService> _usersService;
        private readonly Mock<ICloudTaskService> _cloudTaskService;
        private ContactsService _target;

        public ContactsServiceTests()
        {
            _contactRepository = new Mock<ISubCollectionRepository<Contact>>();
            _inviteRepository = new Mock<ICollectionRepository<Invite>>();
            _usersService = new Mock<IUserService>();
            _cloudTaskService = new Mock<ICloudTaskService>();

            _target = new ContactsService(
                _contactRepository.Object,
                _inviteRepository.Object,
                _usersService.Object,
                _cloudTaskService.Object,
                Mock.Of<ILogger<ContactsService>>()
            );


            _contactRepository.Setup(m => m.GetByFieldAsync(
                            It.IsAny<string>(), nameof(Contact.PhoneNumber),
                            It.IsAny<string>())).ReturnsAsync(new List<Contact> { });

            _inviteRepository.Setup(m => m.GetByFieldAsync(
                nameof(Invite.FromUserPhoneNumber),
                It.IsAny<string>())).ReturnsAsync(new List<Invite> { });

            _inviteRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Invite());

            _usersService.Setup(m => m.GetPersonalInformationAsync(It.IsAny<string>()))
                .ReturnsAsync(new PersonalInformation());
        }

        [Fact]
        public async Task InsertManyAsync_should_not_add_existing_contacts()
        {
            // Arrange
            _contactRepository.Setup(m => m.GetByFieldAsync(
                            It.IsAny<string>(), nameof(Contact.PhoneNumber),
                            "0001")).ReturnsAsync(new List<Contact> { new Contact() });

            // Act
            await _target.InsertManyAsync(string.Empty, new List<BasicContactInfo>
            {
                new BasicContactInfo
                {
                    PhoneNumber = "0001"
                }
            });

            // Assert
            _inviteRepository.Verify(m => m.InsertAsync(It.IsAny<string>(), It.IsAny<Invite>()), Times.Never);
        }

        [Fact]
        public async Task InsertManyAsync_should_add_contact_of_existing_requests()
        {
            // Arrange
            _inviteRepository.Setup(m => m.GetByFieldAsync(
                           nameof(Invite.FromUserPhoneNumber),
                           "0001")).ReturnsAsync(new List<Invite> {
                               new Invite()
                               {
                                   InviteId = "invite"
                               }
                           });

            // Act
            await _target.InsertManyAsync(string.Empty, new List<BasicContactInfo>
            {
                new BasicContactInfo
                {
                    PhoneNumber = "0001"
                }
            });

            // Assert
            _contactRepository.Verify(m => m.InsertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Contact>()), Times.Exactly(2));
            _inviteRepository.Verify(m => m.DeleteAsync("invite"), Times.Once);
            _inviteRepository.Verify(m => m.InsertAsync(It.IsAny<string>(), It.IsAny<Invite>()), Times.Never);
        }

        [Fact]
        public async Task InsertManyAsync_should_add_invite()
        {
            // Act
            await _target.InsertManyAsync(string.Empty, new List<BasicContactInfo>
            {
                new BasicContactInfo
                {
                    PhoneNumber = "0001"
                }
            });

            // Assert
            _contactRepository.Verify(m => m.InsertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Contact>()), Times.Never);
            _inviteRepository.Verify(m => m.DeleteAsync("invite"), Times.Never);
            _inviteRepository.Verify(m => m.InsertAsync(It.IsAny<string>(), It.IsAny<Invite>()), Times.Once);
        }

        public void Dispose()
        {
            _target = null;
        }
    }
}
