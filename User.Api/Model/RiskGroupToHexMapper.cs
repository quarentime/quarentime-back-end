using System.Collections.Generic;

namespace User.Api.Model
{
    public static class RiskGroupToHexMapper
    {
        public static IDictionary<RiskGroup, string> HexMapper { get; } = new Dictionary<RiskGroup, string>
        {
            { RiskGroup.Positive, "#F76161" },
            { RiskGroup.Recovered, "#C18EBD" },
            { RiskGroup.LowProbabilitySuspected, "#F7BA61" },
            { RiskGroup.HighProbabilitySuspected, "#F7BA61" },
            { RiskGroup.Healthy, "#61C1F7" },
            { RiskGroup.FluLike, "#61C1F7" },
            { RiskGroup.HealtySocialDistancing, "#61C1F7" }
        };
    }
}
