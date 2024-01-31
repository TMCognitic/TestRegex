using System.Text.RegularExpressions;

namespace TestRegex
{
    public class UnitTest1
    {
        [Fact]
        public void BadBase64StringFormat()
        {
            string base64String = "Azerty==";

            bool result = RegexHelpers.Match(base64String, out string? resultString, out string? content);

            Assert.False(result);
            Assert.Null(resultString);
            Assert.Null(content);
        }

        [Fact]
        public void TestJpeg()
        {
            string base64String = "data:image/jpeg;base64,Azerty==";

            bool result = RegexHelpers.Match(base64String, out string? resultString, out string? content);

            Assert.True(result);
            Assert.Equal("image/jpeg", resultString);
            Assert.Equal("Azerty==", content);
        }

        [Fact]
        public void TestPng()
        {
            string base64String = "data:image/png;base64,Azerty==";

            bool result = RegexHelpers.Match(base64String, out string? resultString, out string? content);

            Assert.True(result);
            Assert.Equal("image/png", resultString);
            Assert.Equal("Azerty==", content);
        }

        [Fact]
        public void TestBmp()
        {
            string base64String = "data:image/bmp;base64,Azerty==";

            bool result = RegexHelpers.Match(base64String, out string? resultString, out string? content);

            Assert.False(result);
            Assert.Null(resultString);
            Assert.Null(content);
        }

        [Fact]
        public void BadBase64StringContent()
        {
            string base64String = "data:image/jpeg;base64,$$$$$==";

            bool result = RegexHelpers.Match(base64String, out string? resultString, out string? content);

            Assert.False(result);
            Assert.Null(resultString);
            Assert.Null(content);
        }
    }

    static class RegexHelpers
    {
        private static Regex regex = new Regex("^data:(?<imageType>image/(jpeg|png));base64,(?<content>[-A-Za-z0-9+/]*={0,3})$");

        public static bool Match(string input, out string? imageType, out string? content)
        {
            ArgumentNullException.ThrowIfNull(input);
            imageType = null;
            content = null;

            Match match = regex.Match(input);

            if(!match.Success)
                return false;
                
            imageType = match.Groups["imageType"].Value;
            content = match.Groups["content"].Value;

            return match.Success;
        }
    }
}