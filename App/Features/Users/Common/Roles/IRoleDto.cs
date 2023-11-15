﻿namespace App.Features.Users.Common.Roles;

public interface IRoleDto
{
    string DataVersion { get; set; }
    string Description { get; set; }
    string Id { get; set; }
    string Name { get; set; }
    ICollection<IUserRoleDto> UserRoles { get; set; }
}
