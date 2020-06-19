using System;
using System.ComponentModel;
using System.Linq;

namespace lalupa.Authorization
{
    public static class PermissionValidator
    {
        public static bool ThisPermissionIsAllowed(this string packedPermissions, string permissionName)
        {
            var usersPermissions = packedPermissions.UnpackPermissionsFromString().ToArray();

            if (!Enum.TryParse(permissionName, true, out PermissionsEnum permissionToCheck))
                throw new InvalidEnumArgumentException($"{permissionName} could not be converted to a {nameof(PermissionsEnum)}.");

            return usersPermissions.Contains(permissionToCheck);
        }
    }
}
