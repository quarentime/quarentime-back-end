using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Api.Model;
using System.Linq;
using System;

namespace User.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        [HttpPost]
        public async Task<UserModel> Post(UserModel value)
        {
            var userId = User.Claims.First(c => c.Type == "user_id");
            value.Id = userId.Value;
            return await Task.FromResult(value);
        }
    }
}