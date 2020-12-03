using demoAPI.Common.Enum;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _6.demoAPI.Common.Security
{
    public class CustomAuthoRequire : IAuthorizationRequirement
    {
        public List<UserRoleEnum> AppceptUserTypes { get; set; }

        public CustomAuthoRequire(string policyName = "")
        {
            this.AppceptUserTypes = new List<UserRoleEnum>() { UserRoleEnum.Administrator };
            AppceptUserTypes.AddRange((IEnumerable<UserRoleEnum>)RoleHandle.GetRoles(policyName));
        }
    }

    public static class RoleHandle
    {
        public static List<UserRoleEnum> GetRoles(string roleStrings)
        {
            var result = new List<UserRoleEnum>() { };
            try
            {
                if (!string.IsNullOrEmpty(roleStrings))
                {
                    var listRole = roleStrings.Split(",");

                    foreach (var role in listRole.Distinct().ToList())
                    {
                        result.Add((UserRoleEnum)Enum.Parse(typeof(UserRoleEnum), role));
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
    }
}