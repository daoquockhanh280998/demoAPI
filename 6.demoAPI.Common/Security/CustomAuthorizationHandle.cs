using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6.demoAPI.Common.Security
{
    public class CustomAuthorizationHandle : AuthorizationHandler<CustomAuthoRequire>
    {
        //    //protected IAuthozirationUtility _authozirationUtility;

        //    //public CustomAuthorizationHandle(IAuthozirationUtility authozirationUtility)
        //    //{
        //    //    _authozirationUtility = authozirationUtility;
        //    //}

        //    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthoRequire requirement)
        //    {
        //        try
        //        {
        //        //    var jwtToken = _authozirationUtility.GetRequestAccessToken(context);
        //         //   var type = jwtToken.GetClaimValue(JwtRegisteredClaimNames.Typ.ToString());
        //           // var userRoles = RoleHandle.GetRoles(type);

        //            var currentAcceptRoles = userRoles.Select(x => (int)x).FirstOrDefault(y => requirement.AppceptUserTypes.Select(z => (int)z).Contains(y));

        //            if (currentAcceptRoles > 0)
        //                context.Succeed(requirement);
        //            else
        //                context.Fail();

        //            return Task.FromResult(0);
        //        }
        //        catch
        //        {
        //            context.Fail();
        //            return Task.FromResult(0);
        //        }
        //    }
        //}
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthoRequire requirement)
        {
            throw new NotImplementedException();
        }
    }
}