using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Domain.Extensions;

public static class EnumerableRowLevelAccessExtensions
{
    public static IEnumerable<string> FilterRowLevelAccess(this IEnumerable<RowLevelAccess> rowLevelAccesses, string parm, AxTable axTable)
    {
        List<string> result;
        
        var parmTrimmed = parm.RemoveSpecialCharacters();
        var filters = rowLevelAccesses.Where(x => x.AxTable == axTable).Select(x => x.Query.RemoveSpecialCharacters()).ToList();
        if (filters.Count != 0 || !string.IsNullOrEmpty(parmTrimmed))
        {
            if (filters.Count != 0 && !string.IsNullOrEmpty(parmTrimmed))
            {
                result = filters.FilterStringsByPrefix(parmTrimmed)
                    .Select(x => $"{x}*")
                    .ToList();

                if (result.Count != 0)
                {
                    return result;
                }

                var leftmostCommonCharacters = filters
                    .Select(item => item.CalculateLeftmostCommonCharacters(parmTrimmed))
                    .FirstOrDefault();
                    
                if (!string.IsNullOrEmpty(leftmostCommonCharacters))
                {
                    result.Add($"{parmTrimmed}*");
                }
            }
            else if (filters.Count != 0)
            {
                result = filters.Select(x => $"{x}*").ToList();
            }
            else
            {
                result = [$"{parm}*"];
            }
        }
        else
        {
            result = ["*"];
        }

        return result;
    }
}