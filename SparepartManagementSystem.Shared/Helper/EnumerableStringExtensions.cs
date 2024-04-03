namespace SparepartManagementSystem.Shared.Helper;

public static class EnumerableStringExtensions
{
    public static IEnumerable<string> FilterLongestCommonPrefixes(this IEnumerable<string> enumerable)
    {
        var list = enumerable as string[] ?? enumerable.ToArray();
        var result = (from item in list let isLongest = list.All(innerItem => item == innerItem || !innerItem.StartsWith(item) || item.Length >= innerItem.Length) where isLongest select item).ToList();
        return result.Distinct().ToList();
    }
    
    public static IEnumerable<string> FilterShortestCommonPrefixes(this IEnumerable<string> enumerable)
    {
        var list = enumerable as string[] ?? enumerable.ToArray();
        var result = (from item in list let isShortest = list.All(innerItem => item == innerItem || !item.StartsWith(innerItem) || item.Length <= innerItem.Length) where isShortest select item).ToList();
        return result.Distinct().ToList();
    }
    
    public static IEnumerable<string> FilterAndFormatStringsByPrefix(this IEnumerable<string> enumerable, string parm)
    {
        var array = enumerable as string[] ?? enumerable.ToArray();
        var filteredList = array
            .Select(item => item[^1].Equals('*') ? item.RemoveSpecialCharacters() + "*" : item.RemoveSpecialCharacters())
            .FilterShortestCommonPrefixes()
            .Where(item => item.StartsWith(parm))
            .Select(item => array.Contains(item + "*") ? item + "*" : item)
            .ToList();

        if (filteredList.Any() || string.IsNullOrEmpty(parm)) return filteredList;
        {
            var leftmostCommonCharacters = array
                .Select(item => item.TrimEnd('*').CalculateLeftmostCommonCharacters(parm))
                .FirstOrDefault(item => array.Contains(item + "*"));

            if (!string.IsNullOrEmpty(leftmostCommonCharacters))
            {
                filteredList.Add(parm + "*");
            }
        }

        return filteredList;
    }
}