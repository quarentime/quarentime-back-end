using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerResponse(200, type: typeof(Response<PersonalInformation>))]
        public async Task<IActionResult> GetPersonalInfoAsync()
        {
            var result = await _userService.GetPersonalInformationAsync(UserId.Value);
            if (result == null)
            {
                throw new NotFoundException();
            }

            return Ok(new Response<PersonalInformation>(result));
        }

        [HttpPost]
        [Route("PersonalInformation")]
        [SwaggerResponse(201, type: typeof(SuccessResponse))]
        public async Task<IActionResult> UpdatePersonalInfoAsync(PersonalInformation value)
        {
            await _userService.UpdatePersonalInformationAsync(UserId.Value, value);

            return Created(new SuccessResponse());
        }

        [HttpPost]
        [Route("DeviceTokenRegistration")]
        public async Task<Response> RegisterUserDeviceToken(string token)
        {
            await _userService.RegisterUserDeviceToken(UserId.Value, token);

            return new SuccessResponse();
        }

        [HttpPost]
        [Route("Survey")]
        [SwaggerResponse(201, type: typeof(Response<SurveyResponse>))]
        public async Task<IActionResult> UpdateSurvey(SurveyIntake value)
        {
            var answer = await _userService.UpdateSurveyInfo(UserId.Value, value);

            return Created(new Response<SurveyResponse>(new SurveyResponse
            {
                Status = answer,
                ColorHex = RiskGroupToHexMapper.HexMapper[answer]
            }));
        }

        [HttpGet]
        [Route("Survey")]
        [SwaggerResponse(200, type: typeof(Response<SurveyIntake>))]
        public async Task<IActionResult> GetSurveyInfo()
        {
            var result = await _userService.GetSurveyInfoAsync(UserId.Value);
            if (result == null)
            {
                throw new NotFoundException();
            }

            return Ok(new Response<SurveyIntake>(result));
        }

        [HttpPost]
        [Route("VerifyPhone")]
        [SwaggerResponse(201, type: typeof(SuccessResponse))]
        public async Task<IActionResult> VerifyPhoneNumber()
        {
            await _userService.RequestPhoneValidation(UserId.Value);
            return Created(new SuccessResponse());
        }

        [HttpPost]
        [Route("ConfirmVerificationCode")]
        [SwaggerResponse(200, type: typeof(Response<bool>))]
        public async Task<IActionResult> ConfirmPhoneNumber(PhoneVerificationContract phoneVerificationContract)
        {
            var result = await _userService.CheckVerificationCode(UserId.Value, phoneVerificationContract.Code);
            return Ok(new Response<bool>(result));
        }

        [HttpPost]
        [Route("Contacts")]
        [SwaggerResponse(201, type: typeof(SuccessResponse))]
        public async Task<IActionResult> AddContacts(ContactCollectionRequestContract contactCollection)
        {
            await _contactsService.InsertManyAsync(UserId.Value, contactCollection.Contacts);
            return Created(new SuccessResponse());
        }

        [HttpGet]
        [Route("Contacts")]
        [SwaggerResponse(200, type: typeof(Response<IEnumerable<Contact>>))]
        public async Task<IActionResult> GetContacts()
        {
            var result = await _contactsService.GetAllContactsAsync(UserId.Value);
            return Ok(new Response<IEnumerable<Contact>>(result));
        }

        [HttpGet]
        [Route("Contacts/Trace")]
        [SwaggerResponse(200, type: typeof(Response<ContactTrace>))]
        public async Task<IActionResult> GetContactTrace()
        {
            var result = await _contactsService.GetContactTrace(UserId.Value);
            return Ok(new Response<ContactTrace>(result));
        }

        [HttpPost]
        [Route("FriendRequests/Accept")]
        [SwaggerResponse(201, type: typeof(SuccessResponse))]
        public async Task<IActionResult> AcceptInvite(AcceptInviteRequestContract invite)
        {
            await _contactsService.AcceptInviteAsync(UserId.Value, invite.InviteId);
            return Created(new SuccessResponse());
        }

        [HttpDelete]
        [Route("FriendRequests/Reject")]
        [SwaggerResponse(200, type: typeof(SuccessResponse))]
        public async Task<IActionResult> RejectInvite(RejectInviteRequestContract invite)
        {
            await _contactsService.RejectInvite(invite.InviteId);
            return Ok(new SuccessResponse());
        }

        [HttpGet]
        [Route("FriendRequests/Pending")]
        [SwaggerResponse(200, type: typeof(Response<IEnumerable<Invite>>))]
        public async Task<IActionResult> GetPendingInvites()
        {
            var result = await _contactsService.GetPendingInvitesAsync(UserId.Value);
            return Ok(new Response<IEnumerable<Invite>>(result));
        }
    }
}