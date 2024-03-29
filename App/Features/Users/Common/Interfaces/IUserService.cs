﻿using App.Features.Users.Common.Models;
using System.Security.Claims;

namespace App.Features.Users.Common.Interfaces;

public interface IUserService
{
	string GetSignedInUserId();
	
	ClaimsPrincipal GetSignedInUser();

	Task UpdateUserModelAsync(UserDto userBasedOnProviderDataDto, Claim authenticationSchemeClaim);

    Task AddNewUserAsync(UserDto userDto);

    Task SetRolesForUserPrincipleAsync(string userId, ClaimsIdentity identity);
}