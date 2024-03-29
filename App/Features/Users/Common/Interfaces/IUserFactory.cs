﻿using App.Common.Interfaces;
using App.Features.Users.Common.Models;
using App.Features.Users.Common.Roles.Models;

namespace App.Features.Users.Common.Interfaces;

public interface IUserFactory : IBaseEntityFactory<UserModel, UserDto>
{
    public UserRoleModel CreateUserRoleModel();
    public UserRoleDto CreateUserRoleDto();
}