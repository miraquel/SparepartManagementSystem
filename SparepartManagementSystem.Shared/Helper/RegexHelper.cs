using System.Text.RegularExpressions;

namespace SparepartManagementSystem.Shared.Helper;

public static partial class RegexHelper
{
    // create a regex helper to avoid magic strings
    [GeneratedRegex("#+")]
    public static partial Regex NumberSequenceRegex();
}