using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace User.Api.Contracts
{
    public class Response
    {
        public Guid RequestId { get; } = Guid.NewGuid();
        public string ErrorCode { get; set; }
    }

    public class Response<T> : Response
    {
        public T Result { get; }

        public Response(T result)
        {
            Result = result;
        }
    }

    public class SucessResponse : Response<string>
    {
        public SucessResponse() : base("sucess")
        {

        }
    }

    public class ValidationResponse : Response<Dictionary<string, string>>
    {
        public ValidationResponse(ModelStateDictionary modelState) : base(new Dictionary<string, string>())
        {
            foreach (var key in modelState.Keys)
            {
                Result.Add(key, string.Join(',', modelState[key].Errors.Select(e => e.ErrorMessage)));
            }
        }

        public ValidationResponse(Dictionary<string, string> errors) : base(errors)
        {

        }
    }
}
