namespace SparepartManagementSystem.Domain.Extensions;

public static class EnumerableUserWarehouseExtensions
{
    public static IEnumerable<string> FilterByParm(this IEnumerable<UserWarehouse> userWarehouses, string parm)
    {
        string[] result;
        
        var parmTrimmed = parm.RemoveSpecialCharacters();
        var filters = userWarehouses.Select(x => x.InventLocationId).ToArray();
        if (filters.Length != 0 || !string.IsNullOrEmpty(parmTrimmed))
        {
            if (filters.Length != 0 && !string.IsNullOrEmpty(parmTrimmed))
            {
                result = filters.FilterStringsByPrefix(parmTrimmed).ToArray();
            }
            else if (filters.Length != 0)
            {
                result = filters.Select(x => x.RemoveSpecialCharacters()).ToArray();
            }
            else
            {
                result = [""];
            }
        }
        else
        {
            result = ["*"];
        }

        return result;
    }
}