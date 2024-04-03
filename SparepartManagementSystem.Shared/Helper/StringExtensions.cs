using System.Text;

namespace SparepartManagementSystem.Shared.Helper;

public static class StringExtensions
{
    public static string CalculateLeftmostCommonCharacters(this string firstString, string secondString)
    {
        var result = new StringBuilder();

        for (var i = 0; i < Math.Min(firstString.Length, secondString.Length); i++)
        {
            if (firstString[i] == secondString[i])
            {
                result.Append(firstString[i]);
            }
            else
            {
                break;
            }
        }

        return result.ToString();
    }
    
    public static string RemoveSpecialCharacters(this string str)
    {
        var sb = new StringBuilder();
        foreach (var c in str.Where(c => c is >= '0' and <= '9' or >= 'A' and <= 'Z' or >= 'a' and <= 'z'))
        {
            sb.Append(c);
        }

        return sb.ToString();
    }
    
    public static int IndexOfFirstSpecialCharacter(this string str)
    {
        for (var i = 0; i < str.Length; i++)
        {
            if (str[i] is < '0' or > '9' && str[i] is < 'A' or > 'Z' && str[i] is < 'a' or > 'z')
            {
                return i;
            }
        }

        return -1;
    }
}