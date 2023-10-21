﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Project_Main.Infrastructure.Helpers;
using Project_Main.Models.ViewModels.InputModels;
using Project_Main.Services.Identity;
using Project_Main.Infrastructure.DTOs;
using Project_Main.Services.DTO;

namespace Project_Main.Controllers
{
    /// <summary>
    /// Controller to manage availability of page's resources via Authentication.
    /// </summary>
    [Authorize]
	[AllowAnonymous]
	public class AccountController : Controller
	{
		private readonly ILogger<AccountController> _logger;
		private readonly ILoginService _loginService;
		private readonly IUserRegisterService _userRegisterService;
		private readonly IUserAuthenticationService _userAuthenticationService;
		private readonly ILogoutService _logoutService;

		private string operationName = string.Empty;
		private readonly string controllerName = nameof(AccountController);

		public AccountController(ILoginService loginService, IUserRegisterService userRegisterService, ILogger<AccountController> logger, 
			IUserAuthenticationService userAuthenticationService, ILogoutService logoutService)
		{
			_loginService = loginService;
			_userRegisterService = userRegisterService;
			_logger = logger;
			_userAuthenticationService = userAuthenticationService;
			_logoutService = logoutService;
		}

		/// <summary>
		/// Return Index of the whole page that is Login Page.
		/// </summary>
		/// <returns>Return user to Login Page.</returns>
		[HttpGet]
		public ViewResult Login() { return View(); }

		/// <summary>
		/// Method allows to login user with provided data by form.
		/// </summary>
		/// <param name="loginInputVM">Model with provided login data.</param>
		/// <returns>Redirect user to specific index view or to login view if authentication failed.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginInputVM loginInputVM)
		{
			if (ModelState.IsValid)
			{
				LoginInputDto loginInputDto = AccountDtoService.TransferToLoginInputDto(loginInputVM);
				bool isLoginDataInvalid = LoginDataValidator.Valid(loginInputDto);

				if (isLoginDataInvalid)
				{
					ModelState.AddModelError(string.Empty, Messages.InvalidLoginData);
					return View(loginInputVM);
				}

				try 
				{
					bool isNotUserRegisteredInDb = !await _loginService.CheckIsUserAlreadyRegisteredAsync(loginInputDto);

					if (isNotUserRegisteredInDb)
					{
						ModelState.AddModelError(string.Empty, Messages.InvalidLoginData);
						return View();
					}

					bool isLoggedInSuccessfully = await _loginService.LogInUserAsync(loginInputDto);

					if (isLoggedInSuccessfully) { return RedirectToAction(BoardsCtrl.BrieflyAction, BoardsCtrl.Name); }
					else
					{
						_logger.LogError(Messages.LoginFailedForRegisteredUser, nameof(Login), loginInputDto.Username);
						ModelState.AddModelError(string.Empty, Messages.UnableToLogin);
						return View();
				}
				}
				catch (Exception ex)
				{
					_logger.LogCritical(ex, Messages.LogExceptionOccurredOnLogging);
					throw;
				}
			}

			return View();
		}

		/// <summary>
		/// Setup returnUrl to main view of app and pass it to challenge url, also with provider name.
		/// </summary>
		/// <param name="provider">Authentication provider name.</param>
		/// <returns>Challenge for a certain Authentication.</returns>
		[HttpGet]
		[Route(CustomRoutes.LoginByProviderRoute)]
		public IActionResult LoginByProvider([FromRoute] string provider)
		{
			if (string.IsNullOrEmpty(provider))
			{
				_logger.LogError(Messages.LogInvalidProviderName);
				throw new ArgumentNullException(nameof(provider));
			}

			var isUserAuthenticated = _userAuthenticationService.AuthenticateUser(User);

			if (isUserAuthenticated)
				return RedirectToAction(BoardsCtrl.BrieflyAction, BoardsCtrl.Name);
			else 
				return _userAuthenticationService.ChallengeProviderToLogin(provider);
		}

		/// <summary>
		/// Method logout user from account.
		/// </summary>
		/// <returns>Return Login View.</returns>
		public async Task<IActionResult> LogOut()
		{
			string userAuthenticationScheme = User.Claims.First(c => c.Type == ConfigConstants.AuthSchemeClaimKey).Value;

			return await _logoutService.LogoutByProviderTypeAsync(this, userAuthenticationScheme);
		}

		/// <summary>
		/// Return Register view.
		/// </summary>
		/// <returns>Return user to Register Page.</returns>
		public ViewResult Register() { return View(); }

		/// <summary>
		/// Method allows to register new user identity.
		/// </summary>
		/// <param name="registerInputVM">Model with provided register data.</param>
		/// <returns>Redirect user to login page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterInputVM registerInputVM)
		{
			if (ModelState.IsValid)
			{
				UserDto userDto = AccountDtoService.TransferToUserDto(registerInputVM);

				operationName = HelperOther.CreateActionNameForLoggingAndExceptions(nameof(Register), controllerName);

				bool isDataInvalid = !UserDtoValidator.ValidData(userDto);

				if (isDataInvalid)
				{
					ModelState.AddModelError(string.Empty, Messages.InvalidRegisterData);
					return View();
				}

				try
				{
					bool isUserRegisteredSuccessfully = await _userRegisterService.RegisterUserAsync(userDto);
					
						if (isUserRegisteredSuccessfully)
							return View(AccountCtrl.LoginAction);
					else
					return View();
				}
				catch (Exception ex)
				{
					_logger.LogCritical(ex, Messages.LogCreatingUserIdentityFailed, operationName);
					return Error();
				}
			}

			return View();
		}

		/// <summary>
		/// Return main error page for production's user.
		/// </summary>
		/// <returns>Return Error View.</returns>
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() { return View(); }
	}
}