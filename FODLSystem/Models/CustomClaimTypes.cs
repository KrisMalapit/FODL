using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace FODLSystem.Models
{
   


    public class CustomClaimTypes
    {
        public const string UserId = "UserId";
        public const string UserName = "UserName";
        public const string FullName = "FullName";
        public const string RoleName = "RoleName";
        public const string CompanyAccess = "CompanyAccess";
        public const string DepartmentID = "DepartmentID";
        public const string DepartmentName = "DepartmentName";
        public const string DispenserAccess = "DispenserAccess";
        public const string LubeAccess = "LubeAccess";

    }
    public static class IdentityExtensions
    {
        public static int GetUserId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.UserId);

            if (claim == null)
                return 0;

            return int.Parse(claim.Value);
        }

        public static string GetUserName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.UserName);

            return claim?.Value ?? string.Empty;
        }
        public static string GetFullName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.FullName);

            return claim?.Value ?? string.Empty;
        }
        public static string GetRoleName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.RoleName);

            return claim?.Value ?? string.Empty;
        }
        public static string GetCompanyAccess(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.CompanyAccess);

            return claim?.Value ?? string.Empty;
        }
        public static string GetDispenserAccess(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.DispenserAccess);

            return claim?.Value ?? string.Empty;
        }
        public static string GetLubeAccess(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.LubeAccess);

            return claim?.Value ?? string.Empty;
        }



        public static string GetDepartmentID(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.DepartmentID);

            return claim?.Value ?? string.Empty;
        }
        public static string GetDepartmentName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.DepartmentName);

            return claim?.Value ?? string.Empty;
        }

    }
}
