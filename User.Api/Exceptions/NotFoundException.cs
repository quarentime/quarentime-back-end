using System.Net;

namespace User.Api.Exceptions
{
    public class NotFoundException : KnownException
    {
        public NotFoundException() : base("not_found", HttpStatusCode.NotFound)
        {
        }
    }
}
