
using System.ComponentModel.DataAnnotations;

namespace lalupa.Authorization
{
    public enum PermissionsEnum : short
    {
        NotSet = 0,

        [Display( Name = "CreateUser", Description = "Can create user")]
        CreateUser = 0x10,
        [Display( Name = "EditUser", Description = "Can edit user")]
        EditUser = 0x11,
        [Display(Name = "DeleteUser", Description = "Can delete user")]
        DeleteUser = 0x12
    }
}
