using System;

namespace RiskProfile.Api.Model
{
    public class RiskProfileModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public RiskGroup RiskGroup { get; set; }
        public int Fever { get; set; }
        public int Cough { get; set; }
        public bool HasBreathingDifficulty { get; set; }
    }
}
