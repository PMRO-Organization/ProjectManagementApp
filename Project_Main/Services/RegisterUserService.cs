﻿using Project_IdentityDomainEntities;
using Project_Main.Infrastructure.Helpers;
using Project_Main.Models.DataBases.Identity;
using Project_Main.Models.DataBases.Identity.DbSetup;

namespace Project_Main.Services
{
	public class RegisterUserService : IRegisterUserService
	{
		private readonly IIdentityUnitOfWork _identityUnitOfWork;
		private readonly IUserRepository _userRepository;

		public RegisterUserService(IIdentityUnitOfWork identityUnitOfWork)
		{
			_identityUnitOfWork = identityUnitOfWork;
			_userRepository = _identityUnitOfWork.UserRepository;
		}

		public async Task<bool> IsPossibleToRegisterUserByProvidedData(string userName)
		{
			bool isNameTaken = await _userRepository.IsNameTakenAsync(userName);

			if (isNameTaken)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> RegisterUserAsync(string userName, string userPassword, string userEmail)
		{
			UserModel newUser = new()
			{
				Email = userEmail,
				FirstName = userName,
				Lastname = userName,
				Password = userPassword,
				Provider = ConfigConstants.DefaultScheme,
				Username = userName,
			};

			IRoleRepository roleRepository = _identityUnitOfWork.RoleRepository;
			RoleModel? roleForNewUser = await roleRepository.GetSingleByFilterAsync(role => role.Name == IdentitySeedData.DefaultRole);

			if (roleForNewUser is null)
			{
				// TODO logger
				throw new InvalidOperationException("no role in db for new user!");
			}

			newUser.NameIdentifier = newUser.UserId;
			newUser.UserRoles.Add(new UserRoleModel()
			{
				User = newUser,
				UserId = newUser.UserId,
				Role = roleForNewUser,
				RoleId = roleForNewUser.Id,
			});

			Task.WaitAny(Task.Run(async () =>
			{
				await _userRepository.AddAsync(newUser);
				await _identityUnitOfWork.SaveChangesAsync();
			}));

			return true;
		}
	}
}