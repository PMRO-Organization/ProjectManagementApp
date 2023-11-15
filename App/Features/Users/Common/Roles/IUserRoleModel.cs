﻿using App.Features.Users.Interfaces;

namespace App.Features.Users.Common.Roles;

public interface IUserRoleModel
{
	IRoleModel Role { get; set; }
	string RoleId { get; set; }
	IUserModel User { get; set; }
	string UserId { get; set; }
}