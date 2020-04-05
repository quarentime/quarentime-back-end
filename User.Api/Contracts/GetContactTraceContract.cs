using Quarentime.Common.Contracts;

namespace User.Api.Contracts
{
    public class GetContactTraceContract : BaseFilterContract
    {   
        public bool DirectOnly { get; set; }
    }
}
