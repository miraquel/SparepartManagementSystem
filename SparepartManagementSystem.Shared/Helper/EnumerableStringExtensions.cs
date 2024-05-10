namespace SparepartManagementSystem.Shared.Helper;

public static class EnumerableStringExtensions
{
    private static IEnumerable<string> FilterShortestCommonPrefixes(this IEnumerable<string> enumerable)
    {
        var list = enumerable as string[] ?? enumerable.ToArray();
        var result = (from item in list let isShortest = list.All(innerItem => item == innerItem || !item.StartsWith(innerItem) || item.Length <= innerItem.Length) where isShortest select item).ToList();
        return result.Distinct();
    }
    
    public static IEnumerable<string> FilterStringsByPrefix(this IEnumerable<string> enumerable, string parm)
    {
        var parmTrimmed = parm.RemoveSpecialCharacters();
        var array = enumerable as string[] ?? enumerable.ToArray();
        var filteredList = array
            .Select(x => x.RemoveSpecialCharacters())
            .FilterShortestCommonPrefixes()
            .Where(item => item.StartsWith(parmTrimmed))
            .ToList();
    
        return filteredList;
    }
}