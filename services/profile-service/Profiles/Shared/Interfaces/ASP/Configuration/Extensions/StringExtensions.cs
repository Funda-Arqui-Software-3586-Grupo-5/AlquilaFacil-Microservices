using System.Text.RegularExpressions;

namespace Profiles.Shared.Interfaces.ASP.Configuration.Extensions;

public static partial class StringExtensions
{
    public static string ToKebabCase(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return KebabCaseRegex().Replace(text, "-$1")
            .Trim()
            .ToLower();
    }
    
    private static Regex KebabCaseRegex()
    {
        return new Regex(@"([a-z])([A-Z])", RegexOptions.Compiled);
    }
}