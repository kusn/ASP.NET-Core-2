using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
        private readonly ILogger<AccountController> _Logger;

        public AccountController(
            UserManager<User> UserManager, SignInManager<User> SignInManager,
            ILogger<AccountController> logger
            )
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
            _Logger = logger;   // будут помеченны как WebStore.Controllers.AccountController
        }

        #region Register

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }
                
        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            using (_Logger.BeginScope("Регистрация пользователя {UserName}", viewModel.UserName))
            {
                var user = new User
                {
                    UserName = viewModel.UserName,
                };

                //_Logger.LogInformation("Регистрация пользователя {0}", user.UserName);
                _Logger.LogInformation("Регистрация пользователя {UserName}", user.UserName);
                //_Logger.LogInformation($"Регистрация пользователя {user.UserName}");    // так делать не надо

                var register_result = await _UserManager.CreateAsync(user, viewModel.Password);
                if (register_result.Succeeded)
                {
                    _Logger.LogInformation("Пользователь {0} успешно зарегестрирован", user.UserName);

                    await _UserManager.AddToRoleAsync(user, Role.Users);

                    _Logger.LogInformation("Пользователю {0} назначена роль {1}", user.UserName, Role.Users);

                    await _SignInManager.SignInAsync(user, false);

                    _Logger.LogInformation("Пользователь {0} вошёл в систему после регистрации", user.UserName);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in register_result.Errors)
                    ModelState.AddModelError("", error.Description);

                _Logger.LogWarning("Ошибка при регистрации пользователя {0}: {1}", user.UserName, string.Join(",", register_result.Errors.Select(err => err.Description)));
            }

                return View(viewModel);
        }

        #endregion

        #region Login

        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = ReturnUrl });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var login_result = await _SignInManager.PasswordSignInAsync(
                viewModel.UserName,
                viewModel.Password,
                viewModel.RememberMe,
                false);

            if (login_result.Succeeded)
            {
                _Logger.LogInformation("Пользователь {0} успешно вошел в систему", viewModel.UserName);
                
                //return Redirect(viewModel.ReturnUrl);   //Не безопасно!!!
                //if(Url.IsLocalUrl(viewModel.ReturnUrl))
                //    return Redirect(viewModel.ReturnUrl);
                //return RedirectToAction("Index", "Home");
                return LocalRedirect(viewModel.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Ошибка ввода имени пользователя или пароля");

            _Logger.LogWarning("Ошибка ввода имени пользователя или пароля при входе {0}", viewModel.UserName);

            return View(viewModel);
        }

        #endregion

        public async Task<IActionResult> Logout()
        {
            var user_name = User.Identity.Name;
            await _SignInManager.SignOutAsync();

            _Logger.LogInformation("Пользователь {0} вышел из системы", user_name);

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccsessDenied()
        {
            _Logger.LogWarning("Отказано в доступе {0} к uri:{1}", User.Identity.Name, HttpContext.Request.Path);
            return View();
        }
    }
}
