﻿using demoAPI.Common.Enum;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _6.demoAPI.Common.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorization : Attribute, IAuthorizeData
    {
        public CustomAuthorization(params object[] Roles)
        {
            if (Roles.Any(p => p.GetType().BaseType != typeof(Enum)))
            {
                this.Policy = Enum.GetName(typeof(UserRoleEnum), UserRoleEnum.Administrator);
            }
            else
            {
                this.Policy = string.Join(",", Roles.Select(p => Enum.GetName(p.GetType(), p)).Distinct().ToList());
            }
        }

        public string Policy { get; set; }
        public string Roles { get; set; }
        public string AuthenticationSchemes { get; set; }
    }
}