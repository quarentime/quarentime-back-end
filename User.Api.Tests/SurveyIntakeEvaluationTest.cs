using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using User.Api.Model;
using User.Api.Services;
using Xunit;

namespace User.Api.Tests
{
    public class SurveyIntakeEvaluationTest
    {
        private static Mock<ICollectionRepository<User.Api.Model.User>> _userRepository;
        private static Mock<ICollectionRepository<SurveyIntake>> _surveyRepository;
        private static Mock<ICollectionRepository<PersonalInformation>> _personalInfoRepository;
        private static Mock<IPhoneVerificationService> _phoneVerificationService;

        public SurveyIntakeEvaluationTest()
        {
            _userRepository = new Mock<ICollectionRepository<Model.User>>();
            _personalInfoRepository = new Mock<ICollectionRepository<PersonalInformation>>();
            _surveyRepository = new Mock<ICollectionRepository<SurveyIntake>>();
            _surveyRepository.Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<SurveyIntake>())).Verifiable();
            _phoneVerificationService = new Mock<IPhoneVerificationService>();
        }


        [Theory, MemberData(nameof(PositiveData))]
        public async Task SurveyIntakeEvaluation_PositiveRiskGroup(SurveyIntake intake)
        {
            var userService = new UserService(_userRepository.Object,
                                              _personalInfoRepository.Object, _surveyRepository.Object,
                                              _phoneVerificationService.Object);

            var result = await userService.UpdateSurveyInfo("", intake);

            Assert.Equal(RiskGroup.Positive, result);
        }

        [Theory]
        [MemberData(nameof(RecoveredData))]
        public async Task SurveyIntakeEvaluation_RecoveredRiskGroup(SurveyIntake intake)
        {
            var userService = new UserService(_userRepository.Object,
                                              _personalInfoRepository.Object, _surveyRepository.Object,
                                              _phoneVerificationService.Object);

            var result = await userService.UpdateSurveyInfo("", intake);

            Assert.Equal(RiskGroup.Recovered, result);
        }

        [Theory]
        [MemberData(nameof(HealthyData))]
        public async Task SurveyIntakeEvaluation_HealthyRiskGroup(SurveyIntake intake)
        {
            var userService = new UserService(_userRepository.Object,
                                              _personalInfoRepository.Object, _surveyRepository.Object,
                                              _phoneVerificationService.Object);

            var result = await userService.UpdateSurveyInfo("", intake);

            Assert.Equal(RiskGroup.Healthy, result);
        }

        [Theory]
        [MemberData(nameof(HealthyKeepSocialDistanceData))]
        public async Task SurveyIntakeEvaluation_HealthyKeepSocialDistanceRiskGroup(SurveyIntake intake)
        {
            var userService = new UserService(_userRepository.Object,
                                              _personalInfoRepository.Object, _surveyRepository.Object,
                                              _phoneVerificationService.Object);

            var result = await userService.UpdateSurveyInfo("", intake);

            Assert.Equal(RiskGroup.HealtySocialDistancing, result);
        }

        [Theory]
        [MemberData(nameof(FluLikeData))]
        public async Task SurveyIntakeEvaluation_FluLikeRiskGroup(SurveyIntake intake)
        {
            var userService = new UserService(_userRepository.Object,
                                              _personalInfoRepository.Object, _surveyRepository.Object,
                                              _phoneVerificationService.Object);

            var result = await userService.UpdateSurveyInfo("", intake);

            Assert.Equal(RiskGroup.FluLike, result);
        }

        [Theory]
        [MemberData(nameof(HighProbabilityCloseContactData))]
        [MemberData(nameof(HighProbabilityRecentTravelData))]
        public async Task SurveyIntakeEvaluation_HighProbabilityRiskGroup(SurveyIntake intake)
        {
            var userService = new UserService(_userRepository.Object,
                                              _personalInfoRepository.Object, _surveyRepository.Object,
                                              _phoneVerificationService.Object);

            var result = await userService.UpdateSurveyInfo("", intake);

            Assert.Equal(RiskGroup.HighProbabilitySuspected, result);
        }

        [Theory]
        [MemberData(nameof(LowProbabilityData))]
        public async Task SurveyIntakeEvaluation_LowProbabilityRiskGroup(SurveyIntake intake)
        {
            var userService = new UserService(_userRepository.Object,
                                              _personalInfoRepository.Object, _surveyRepository.Object,
                                              _phoneVerificationService.Object);

            var result = await userService.UpdateSurveyInfo("", intake);

            Assert.Equal(RiskGroup.LowProbabilitySuspected, result);
        }

        public static IEnumerable<object[]> PositiveData
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new SurveyIntake
                        {
                            IsTestedPositive = true,
                            HasRecovered = false
                        }
                    }
                };
            }
        }
        public static IEnumerable<object[]> RecoveredData
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new SurveyIntake
                        {
                            IsTestedPositive = true,
                            HasRecovered = true
                        }
                    }
                };
            }
        }

        public static IEnumerable<object[]> HealthyData
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new SurveyIntake
                        {
                            IsTestedPositive = false,
                            Symptoms = new List<Symptom>(),
                            HasRecentTravelLast14Days = false
                        }
                    }
                };
            }
        }

        public static IEnumerable<object[]> HealthyKeepSocialDistanceData
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new SurveyIntake
                        {
                            IsTestedPositive = false,
                            Symptoms = new List<Symptom>(),
                            HasRecentTravelLast14Days = true
                        }
                    }
                };
            }
        }

        public static IEnumerable<object[]> LowProbabilityData
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new SurveyIntake
                        {
                            IsTestedPositive = false,
                            Symptoms = new List<Symptom>() { Symptom.DryCough },
                            HasRecentTravelLast14Days = true
                        }
                    }
                };
            }
        }

        public static IEnumerable<object[]> HighProbabilityRecentTravelData
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new SurveyIntake
                        {
                            IsTestedPositive = false,
                            Symptoms = new List<Symptom>() { Symptom.Fever, Symptom.DryCough, Symptom.Fatigue },
                            HasRecentTravelBeforeSymptoms = true
                        }
                    }
                };
            }
        }

        public static IEnumerable<object[]> FluLikeData
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new SurveyIntake
                        {
                            IsTestedPositive = false,
                            Symptoms = new List<Symptom>() { Symptom.Fever, Symptom.DryCough, Symptom.ThroatPainRunnyNose },
                            HasRecentTravelBeforeSymptoms = false,
                            HasCloseContact = false
                        }
                    }
                };
            }
        }

        public static IEnumerable<object[]> HighProbabilityCloseContactData
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new SurveyIntake
                        {
                            IsTestedPositive = false,
                            Symptoms = new List<Symptom>() { Symptom.Fever, Symptom.DryCough, Symptom.ThroatPainRunnyNose },
                            HasRecentTravelBeforeSymptoms = false,
                            HasCloseContact = true
                        }
                    }
                };
            }
        }

    }
}
