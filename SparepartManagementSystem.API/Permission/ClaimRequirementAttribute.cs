using Microsoft.AspNetCore.Mvc;

namespace SparepartManagementSystem.API.Permission
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(List<string> permissions) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { permissions };
        }
    }
}
