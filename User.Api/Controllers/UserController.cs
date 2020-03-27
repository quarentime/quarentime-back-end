using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.Api.Contracts;
using User.Api.Exceptions;
using User.Api.Model;
using User.Api.Services;

namespace User.Api.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IContactsService _contactsService;

        public UserController(IUserService userService, IContactsService contactsService)
        {
            _userService = userService;
            _contactsService = contactsService;
        }

        [HttpGet]
        [Route("PersonalInformation")]
        public async Task<Response<PersonalInformation>> GetPersonalInfoAsync()
        {
            var result = await _userService.GetPersonalInformationAsync(UserId.Value);
            if (result == null)
            {
                throw new NotFoundException();
            }

            return new Response<PersonalInformation>(result);
        }

        [HttpPost]
        [Route("PersonalInformation")]
        public async Task<Response> UpdatePersonalInfoAsync(PersonalInformation value)
        {
            await _userService.UpdatePersonalInformationAsync(UserId.Value, value);

            return new SucessResponse();
        }

        [HttpPost]
        [Route("Survey")]
        public async Task<Response<SurveyResponse>> UpdateSurvey(SurveyIntake value)
        {
            var answer = await _userService.UpdateSurveyInfo(UserId.Value, value);

            return new Response<SurveyResponse>(new SurveyResponse
            {
                Status = answer,
                ColorHex = RiskGroupToHexMapper.HexMapper[answer]
            });
        }

        [HttpGet]
        [Route("Survey")]
        public async Task<Response<SurveyIntake>> GetSurveyInfo()
        {
            var result = await _userService.GetSurveyInfoAsync(UserId.Value);
            if (result == null)
            {
                throw new NotFoundException();
            }

            return new Response<SurveyIntake>(result);
        }

        [HttpPost]
        [Route("VerifyPhone")]
        public async Task<Response> VerifyPhoneNumber()
        {
            await _userService.RequestPhoneValidation(UserId.Value);
            return new SucessResponse();
        }

        [HttpPost]
        [Route("ConfirmVerificationCode")]
        public async Task<Response<bool>> ConfirmPhoneNumber(PhoneVerificationContract phoneVerificationContract)
        {
            var result = await _userService.CheckVerificationCode(UserId.Value,phoneVerificationContract.Code);
            return new Response<bool>(result);
        }

        [HttpPost]
        [Route("Contacts")]
        public async Task<Response> AddContacts(ContactCollectionRequestContract contactCollection)
        {
            await _contactsService.InsertManyAsync(UserId.Value, contactCollection.Contacts);
            return new SucessResponse();
        }

        [HttpGet]
        [Route("Contacts")]
        public async Task<Response<IEnumerable<Contact>>> GetContacts()
        {
            var result = await _contactsService.GetAllContactsAsync(UserId.Value);
            return new Response<IEnumerable<Contact>>(result);
        }

        [HttpPost]
        [Route("Invites/Accept")]
        public async Task<Response> AcceptInvite(AcceptInviteRequestContract invite)
        {
            await _contactsService.AcceptInviteAsync(UserId.Value, invite.InviteId);
            return new SucessResponse();
        }

        [HttpDelete]
        [Route("Invites/Reject}")]
        public async Task<Response> RejectInvite(RejectInviteRequestContract invite)
        {
            await _contactsService.RejectInvite(invite.InviteId);
            return new SucessResponse();
        }

        [HttpGet]
        [Route("Invites/Pending")]
        public async Task<Response<IEnumerable<Invite>>> GetPendingInvites()
        {
            var result = await _contactsService.GetPendingInvitesAsync(UserId.Value);
            return new Response<IEnumerable<Invite>>(result);
        }
    }
}