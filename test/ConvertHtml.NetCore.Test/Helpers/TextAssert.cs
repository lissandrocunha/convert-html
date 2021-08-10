using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace ConvertHtml.NetCore.Test.Helpers
{
    public static class TextAssert
    {
        private static readonly Regex WhitespaceOrLineFeedRegex = new Regex(@"[\s\n\r]");

        public static void Contains(string value, string substring)
        {
            ContainsExact(value.ToLowerCaseSingleLine(), substring.ToLowerCaseSingleLine());
        }

        public static void AreEqual(string expected, string actual)
        {
            AreEqualExact(expected.ToLowerCaseSingleLine(), actual.ToLowerCaseSingleLine());
        }

        private static void AreEqualExact(string expected, string actual)
        {
            Assert.Equal(expected, actual);
        }

        private static void ContainsExact(string value, string substring)
        {
            Assert.Contains(value, substring);
        }

        private static string ToLowerCaseSingleLine(this string text)
        {
            return text
                .ToSingleLine()
                .ToLower();
        }

        private static string ToSingleLine(this string text)
        {
            return WhitespaceOrLineFeedRegex.Replace(text, "");
        }
    }
}
