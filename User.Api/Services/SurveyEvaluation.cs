using System.Collections.Generic;
using User.Api.Model;

namespace User.Api.Services
{
    public static class SurveyEvaluation
    {
        public static RiskGroup Evaluate(this SurveyIntake intake)
        {
            return EvaluateAnswers(intake);
        }

        static RiskGroup EvaluateAnswers(SurveyIntake intake)
        {
            var symptomsCount = intake.Symptoms?.Count ?? 0;

            if (intake.IsTestedPositive)
            {
                if (!intake.HasRecovered)
                    return RiskGroup.Positive;
                return RiskGroup.Recovered;
            }
            else
            {
                if (SymptomsSuspectedCase(intake.Symptoms))
                {
                    if (intake.HasRecentTravelBeforeSymptoms)
                        return RiskGroup.HighProbabilitySuspected;
                    else if (intake.HasCloseContact)
                        return RiskGroup.HighProbabilitySuspected;
                    else
                        return RiskGroup.FluLike;
                }
                else
                {
                    if (!intake.HasRecentTravelLast14Days)
                        return RiskGroup.Healthy;
                    else
                    {
                        if (symptomsCount < 1)
                            return RiskGroup.HealtySocialDistancing;
                        return RiskGroup.LowProbabilitySuspected;
                    }
                }
            }
        }

        static bool SymptomsSuspectedCase(List<Symptom> symptoms)
        {
            if (symptoms.Contains(Symptom.Fever))
            {
                if (symptoms.Count > 1)
                    return true;
            }
            else if (symptoms.Count > 1)
            {
                return true;
            }

            return false;
        }
    }
}
