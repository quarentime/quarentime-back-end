namespace User.Api.Model
{
    public class RiskFactors
    {
        public RiskGroup RiskGroup { get; set; }
        public bool HasRecentTravel { get; set; }
        public bool HasCloseContact { get; set; }
        public bool HasSymptoms { get; set; }
        public bool HasRecovered { get; set; }
    }
}
