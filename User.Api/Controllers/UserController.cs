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
        public UserController(IUserService userService)
        {
            _userService = userService;
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

            return new SuccessResponse();
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
            return new SuccessResponse();
        }

        [HttpPost]
        [Route("ConfirmVerificationCode")]
        public async Task<Response<bool>> ConfirmPhoneNumber(PhoneVerificationContract phoneVerificationContract)
        {
            var result = await _userService.CheckVerificationCode(UserId.Value,phoneVerificationContract.Code);
            return new Response<bool>(result);
        }
    }
}