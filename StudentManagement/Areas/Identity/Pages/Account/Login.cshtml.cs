using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;
using StudentManagement.Utility;

namespace StudentManagement.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (_unitOfWork.Rule.GetAll().Count() == 0)
            {
                Rule rule1 = new Rule()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Sỉ số",
                    Min = 0,
                    Max = 40
                };
                Rule rule2 = new Rule()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Tuổi học sinh",
                    Min = 5,
                    Max = 20
                };
                Rule rule3 = new Rule()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Điểm chuẩn",
                    Min = 5,
                    Max = 10
                };

                _unitOfWork.Rule.Add(rule1);
                _unitOfWork.Rule.Add(rule2);
                _unitOfWork.Rule.Add(rule3);
                _unitOfWork.Save();

            }

            if (_unitOfWork.Subject.GetAll().Count() == 0)
            {
                Subject sub = new Subject() { Id = 1, Name = "Toán" };
                Subject sub1 = new Subject() { Id = 2, Name = "Lý" };
                Subject sub2 = new Subject() { Id = 3, Name = "Hóa" };
                Subject sub3 = new Subject() { Id = 4, Name = "Sinh" };
                Subject sub4 = new Subject() { Id = 5, Name = "Sử" };
                Subject sub5 = new Subject() { Id = 6, Name = "Địa" };
                Subject sub6 = new Subject() { Id = 7, Name = "Văn" };
                Subject sub7 = new Subject() { Id = 8, Name = "Đạo Đức" };
                Subject sub8 = new Subject() { Id = 9, Name = "Thể Dục" };

                _unitOfWork.Subject.Add(sub);
                _unitOfWork.Subject.Add(sub1);
                _unitOfWork.Subject.Add(sub2);
                _unitOfWork.Subject.Add(sub3);
                _unitOfWork.Subject.Add(sub4);
                _unitOfWork.Subject.Add(sub5);
                _unitOfWork.Subject.Add(sub6);
                _unitOfWork.Subject.Add(sub7);
                _unitOfWork.Subject.Add(sub8);
                _unitOfWork.Save();

            }

            if (_unitOfWork.ApplicationUser.GetAll().Count() == 0)
            {
                //var user = new ApplicationUser()
                //{
                //    UserName = "admin@gmail.com",
                //    Email = "admin@gmail.com",
                //    Name = "Nhà phát triển",
                //    PhoneNumber = "0987639079",
                //    Address = "Không",
                //    Role = SD.Role_Admin
                //};
                //var result = await _userManager.CreateAsync(user, Input.Password);
                //if (result.Succeeded)
                //{
                //    if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                //    {
                //        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                //    }
                //    if (!await _roleManager.RoleExistsAsync(SD.Role_Manager))
                //    {
                //        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Manager));
                //    }
                //    if (!await _roleManager.RoleExistsAsync(SD.Role_Teacher))
                //    {
                //        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Teacher));
                //    }
                //    if (!await _roleManager.RoleExistsAsync(SD.Role_Student))
                //    {
                //        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Student));
                //    }
                //    await _userManager.AddToRoleAsync(user, user.Role);
                //}
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
