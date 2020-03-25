using System;
using User.Api.Model;

namespace User.Api.Contracts
{
    public class SurveyResponse
    {
        public RiskGroup Status { get; set; }
        public String ColorHex { get; set; }
    }
}
