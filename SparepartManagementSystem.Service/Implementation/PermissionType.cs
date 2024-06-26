using System.Reflection;
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Implementation;

public class PermissionTypeAccessor
{
    public IEnumerable<PermissionDto> AllPermission { get; } = GetAllPermission();
    public IEnumerable<PermissionDto> AllModule { get; } = GetAllModule();

    private static IEnumerable<PermissionDto> GetAllPermission()
    {
        var permissionType = typeof(PermissionType);
        var members = permissionType.GetNestedTypes();
        var fieldInfos = new List<FieldInfo>();
        foreach (var member in members)
        {
            fieldInfos.AddRange(member.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
        }

        return fieldInfos.Select(fieldInfo => new PermissionDto
        {
            Module = fieldInfo.ReflectedType?.Name ?? string.Empty,
            Type = fieldInfo.Name,
            PermissionName = fieldInfo.GetValue(null)?.ToString() ?? string.Empty
        });
    }

    private static IEnumerable<PermissionDto> GetAllModule()
    {
        var permissionType = typeof(PermissionType);
        var members = permissionType.GetNestedTypes();
        return members.Select(member => new PermissionDto
        {
            Module = member.Name,
        }).ToArray();
    }
}
public static class PermissionType
{
    public static class GoodsReceiptActivity
    {
        public const string Read = "goodsReceipt.Read";
        public const string Create = "goodsReceipt.Create";
        public const string Update = "goodsReceipt.Update";
        public const string Delete = "goodsReceipt.Delete";
        public const string Process = "goodsReceipt.Process";
    }
    public static class GMKSMSServiceGroupActivity
    {
        public const string Read = "gmkSMSServiceGroup.Read";
        public const string Create = "gmkSMSServiceGroup.Create";
        public const string Update = "gmkSMSServiceGroup.Update";
        public const string Delete = "gmkSMSServiceGroup.Delete";
        public const string Process = "gmkSMSServiceGroup.Process";
    }
    public static class NumberSequenceActivity
    {
        public const string Read = "numberSequence.Read";
        public const string Create = "numberSequence.Create";
        public const string Update = "numberSequence.Update";
        public const string Delete = "numberSequence.Delete";
        public const string Process = "numberSequence.Process";
    }

    public static class UserActivity
    {
        public const string Read = "user.Read";
        public const string Create = "user.Create";
        public const string Update = "user.Update";
        public const string Delete = "user.Delete";
        public const string Process = "user.Process";
    }

    public static class RoleActivity
    {
        public const string Read = "role.Read";
        public const string Create = "role.Create";
        public const string Update = "role.Update";
        public const string Delete = "role.Delete";
        public const string Process = "role.Process";
    }

    public static class PermissionActivity
    {
        public const string Read = "rolePermission.Read";
        public const string Create = "rolePermission.Create";
        public const string Update = "rolePermission.Update";
        public const string Delete = "rolePermission.Delete";
        public const string Process = "rolePermission.Process";
    }
    
    public static class RowLevelAccessActivity
    {
        public const string Read = "rowLevelAccess.Read";
        public const string Create = "rowLevelAccess.Create";
        public const string Update = "rowLevelAccess.Update";
        public const string Delete = "rowLevelAccess.Delete";
        public const string Process = "rowLevelAccess.Process";
    }
    
    public static class WorkOrderActivity
    {
        public const string Read = "workOrder.Read";
        public const string Create = "workOrder.Create";
        public const string Update = "workOrder.Update";
        public const string Delete = "workOrder.Delete";
        public const string Process = "workOrder.Process";
    }
    
    public static class UserWarehouseActivity
    {
        public const string Read = "userWarehouse.Read";
        public const string Create = "userWarehouse.Create";
        public const string Update = "userWarehouse.Update";
        public const string Delete = "userWarehouse.Delete";
        public const string Process = "userWarehouse.Process";
    }
    
    public static class VersionTracker
    {
        public const string Read = "versionTracker.Read";
        public const string Create = "versionTracker.Create";
        public const string Update = "versionTracker.Update";
        public const string Delete = "versionTracker.Delete";
        public const string Process = "versionTracker.Process";
    }
}