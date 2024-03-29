﻿using App.Features.Users.Common.Models;
using App.Features.Users.Common.Roles.Models.Interfaces;

namespace App.Features.Users.Common.Roles.Models;

public class UserRoleModel : IUserRoleModel
{
    public UserRoleModel()
    {
        User = new UserModel();
        Role = new RoleModel();
    }

    public UserRoleModel(UserModel userModel, RoleModel roleModel)
    {
        User = userModel;
        Role = roleModel;
    }

    public string UserId { get; set; } = string.Empty;
    public UserModel User { get; set; }

    public string RoleId { get; set; } = string.Empty;
    public RoleModel Role { get; set; }
}
