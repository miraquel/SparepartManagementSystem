using System.Reflection;
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Implementation
{
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
            }).ToArray();
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
        public static class GoodsReceipt
        {
            public const string Read = "goodsReceipt.Read";
            public const string Create = "goodsReceipt.Create";
            public const string Update = "goodsReceipt.Update";
            public const string Delete = "goodsReceipt.Delete";
            public const string Process = "goodsReceipt.Process";
        }
        public static class GMKSMSServiceGroup
        {
            public const string Read = "gmkSMSServiceGroup.Read";
            public const string Create = "gmkSMSServiceGroup.Create";
            public const string Update = "gmkSMSServiceGroup.Update";
            public const string Delete = "gmkSMSServiceGroup.Delete";
            public const string Process = "gmkSMSServiceGroup.Process";
        }
        public static class NumberSequence
        {
            public const string Read = "numberSequence.Read";
            public const string Create = "numberSequence.Create";
            public const string Update = "numberSequence.Update";
            public const string Delete = "numberSequence.Delete";
            public const string Process = "numberSequence.Process";
        }

        public static class User
        {
            public const string Read = "user.Read";
            public const string Create = "user.Create";
            public const string Update = "user.Update";
            public const string Delete = "user.Delete";
            public const string Process = "user.Process";
        }

        public static class Role
        {
            public const string Read = "role.Read";
            public const string Create = "role.Create";
            public const string Update = "role.Update";
            public const string Delete = "role.Delete";
            public const string Process = "role.Process";
        }

        public static class RolePermission
        {
            public const string Read = "rolePermission.Read";
            public const string Create = "rolePermission.Create";
            public const string Update = "rolePermission.Update";
            public const string Delete = "rolePermission.Delete";
            public const string Process = "rolePermission.Process";
        }
    }
}
