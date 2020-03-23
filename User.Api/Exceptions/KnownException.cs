using System;
using System.Net;

namespace User.Api.Exceptions
{
    public class KnownException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public KnownException(string errorCode, HttpStatusCode statusCode):base(errorCode)
        {
            StatusCode = statusCode;
        }
    }
}
