using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Api.Contracts;

namespace User.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController : Controller
    {
        protected Claim UserId => User.Claims.First(c => c.Type == "user_id");

        protected JsonResult Ok(Response response)
        {
            return new JsonResult(response)
            {
                StatusCode = 200
            };
        }

        protected JsonResult Created(Response response)
        {
            return new JsonResult(response)
            {
                StatusCode = 201
            };
        }
     
    }
}