using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.Api.Model;

namespace User.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        [HttpPost]
        public async Task<UserModel> Post(UserModel value)
        {
            return await Task.FromResult(value);
        }
    }
}