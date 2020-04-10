using User.Api.Extensions;
using Xunit;

namespace User.Api.Tests
{
    public class ContactTraceTests
    {
        [Theory]
        [InlineData("abc def", "AD")]
        [InlineData("abc def ghi", "ADG")]
        [InlineData("test_quarentime_user_1_1586502524350 New User 1", "TNU1")]
        public void Field_initials_should_return_correct_values(string input, string expected)
        {
            // Arrange
            var result = StringExtensions.Initials(input);

            Assert.Equal(expected, result);
        }
    }
}
