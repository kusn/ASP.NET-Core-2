using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels.Identity;

namespace WebStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;

        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
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

            var user = new User
            {
                UserName = viewModel.UserName,
            };

            var register_result = await _UserManager.CreateAsync(user, viewModel.Password);
            if (register_result.Succeeded)
            {
                await _SignInManager.SignInAsync(user, false);

                await _UserManager.AddToRoleAsync(user, Role.Users);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in register_result.Errors)
                ModelState.AddModelError("", error.Description);

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
                //return Redirect(viewModel.ReturnUrl);   //Не безопасно!!!
                //if(Url.IsLocalUrl(viewModel.ReturnUrl))
                //    return Redirect(viewModel.ReturnUrl);
                //return RedirectToAction("Index", "Home");
                return LocalRedirect(viewModel.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Ошибка ввода имени пользователя или пароля");

            return View(viewModel);
        }

        #endregion

        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccsessDenied()
        {
            return View();
        }
    }
}
