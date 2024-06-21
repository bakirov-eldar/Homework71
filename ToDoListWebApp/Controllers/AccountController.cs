using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ToDoListWebApp.Models;
using ToDoListWebApp.Services.Email;
using ToDoListWebApp.ViewModels;

namespace ToDoListWebApp.Controllers
{
    public class AccountController : Controller
    {
        const string RegEmailTitle = "Вы успешно зарегистрированы в ToDoList!";
        const string RegEmailText = "Здравствуйте {0}!<br>Вы успешно зарегистрированы в ToDoList!<br>Ваш логин: {1}<br>Ссылка на ваш профиль: <a href=\"{2}\">*ссылка*</a>";
        const string ChangePassTitle = "Ваш пароль изменён!";
        const string ChangePassText = "Здравствуйте {0}!<br>Ваш пароль в ToDoList был изменен!";

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;
        private readonly ToDoListContext _context;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, EmailService emailService, ToDoListContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }
        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProfileData()
        {
            var user = await _userManager.GetUserAsync(User);
            var title = "Ваши запрошенные данные готовы";
            StringBuilder text = new StringBuilder();
            text.AppendLine($"Здравствуйте {user.Firstname}<br>");
            text.AppendLine($"Ваши данные:<br>");
            text.AppendLine($"Ваше имя: {user.Firstname} {user.Lastname}.<br>");
            text.AppendLine($"Указанный email: {user.Email}<br>");
            int ownTasks = await _context.ToDoList.CountAsync(e => e.CreatorId == user.Id);
            text.AppendLine($"Количество ваших задач: {ownTasks}.<br>");
            int performerTasks = await _context.ToDoList.CountAsync(e => e.PerformerId == user.Id);
            text.AppendLine($"Количество выполняемых вами задач: {performerTasks}.<br>");
            _emailService.SendEmail(user.Email, title, text.ToString());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    _emailService.SendEmail(user.Email, ChangePassTitle, string.Format(ChangePassText, user.Firstname));
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Login(string? returnUrl)
        {
            return View(new LoginViewModel() { ReturnUrl = returnUrl});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        return RedirectToAction("Index", "ToDoList");
                    }
                }
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }
        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    UserName = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                    await _signInManager.SignInAsync(user, false);
                    _emailService.SendEmail(user.Email, RegEmailTitle, string.Format(RegEmailText, user.Firstname, user.UserName, Url.ActionLink("Index")));
                    return RedirectToAction("Index", "ToDoList");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            var user = await _userManager.GetUserAsync(User);
            await _userManager.DeleteAsync(user);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Register");
        }
    }
}
