// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrescribingSystem.Data;
using PrescribingSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;


namespace PrescribingSystem.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _configuration = configuration;

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            //[Required]
            //[Display(Name = "Health Council Registration Number")]
            //public string HealthCouncilRegistrationNumber { get; set; }

            [Required]
            [StringLength(13, MinimumLength = 13, ErrorMessage = "SA ID Number must be exactly 13 digits.")]
            [RegularExpression(@"^\d{13}$", ErrorMessage = "SA ID Number must contain only digits.")]
            [Display(Name = "Identity Number")]
            public string IdentityNumber { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Cellphone Number")]
            [RegularExpression(@"^(?:\+27|0)[6-8][0-9]{8}$", ErrorMessage = "Please enter a valid South African phone number.")]
            public string CellphoneNumber { get; set; }

            //[Required]
            //[Display(Name = "Role")]
            //public string Role { get; set; }

            //[ValidateNever]
            //public IEnumerable<SelectListItem> RoleList { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            //Input = new InputModel()
            //{
            //    RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
            //    {
            //        Text = i,
            //        Value = i
            //    })
            //};
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //string generatedPassword = GeneratePassword();

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                //HealthCouncilRegistrationNumber = Input.HealthCouncilRegistrationNumber,
                IdentityNumber = Input.IdentityNumber,
                CellphoneNumber = Input.CellphoneNumber
            };

            //Check if a user with the same IdentityNumber already exists
            var existingUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.IdentityNumber == Input.IdentityNumber);

            if (existingUser != null)
            {
                ModelState.AddModelError("Input.IdentityNumber", "This SA ID Number is already registered.");
                return Page();
            }

            //var result = await _userManager.CreateAsync(user, "Tester@1234");
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                // Add FirstName and LastName as claims
                await _userManager.AddClaimAsync(user, new Claim("FirstName", user.FirstName));
                await _userManager.AddClaimAsync(user, new Claim("LastName", user.LastName));


                await _userManager.AddToRoleAsync(user, "Customer");
                await _signInManager.SignInAsync(user, isPersistent: false);


                return RedirectToAction("AddCustomerAllergies", "Customer", new { id = user.Id });

                //var userId = await _userManager.GetUserIdAsync(user);
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                //var callbackUrl = Url.Page(
                //    "/Account/ConfirmEmail",
                //    pageHandler: null,
                //    values: new { area = "Identity", userId = user.Id, code = code },
                //    protocol: Request.Scheme);

                //await _emailSender.SendEmailAsync(user.Email, "Confirm Your Email",
                //   $"An account with Ibhayi Pharmacy has been created for you. <br>" +
                //   $"Please confirm your email by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.<br><br>" +
                //   $"Your login details: <br>" +
                //   $"Health Council Registration Number: <b>{user.HealthCouncilRegistrationNumber}</b><br>" +
                //   $"Password: <b>{generatedPassword}</b><br>" +
                //   $"You cannot log in until your email is confirmed, please use above details to login once account has been confirmed.");

                //return RedirectToPage("/Account/RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }

        //Improved Mthod to ensure all password requirements are met.
        //private string GeneratePassword(int length = 12)
        //{
        //    const string lowercase = "abcdefghijklmnopqrstuvwxyz";
        //    const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    const string numbers = "0123456789";
        //    const string specialChars = "!@#$%&^*()_+";

        //    var allChars = lowercase + uppercase + numbers + specialChars;
        //    var random = new Random();

        //    //Ensure at least one of each character type
        //    var password = new Char[]
        //    {
        //        lowercase[random.Next(lowercase.Length)],
        //        uppercase[random.Next(uppercase.Length)],
        //        numbers[random.Next(numbers.Length)],
        //        specialChars[random.Next(specialChars.Length)]
        //    };

        //    //Fill the rest of the  password length
        //    for (int i = 7; i < length; i++) 
        //    {
        //        password = password.Append(allChars[random.Next(allChars.Length)]).ToArray();
        //    }

        //    //
        //    return "IBP" + new string(password.OrderBy(x => random.Next()).ToArray());
            
        //    // return $"IBP{Guid.NewGuid().ToString().Substring(0, 9)}!";
        //}

        public async Task<IActionResult> OnGetConfirmEmailAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {

                await _userManager.UpdateAsync(user);

                return RedirectToPage("/Account/ConfirmEmailSuccess");
            }

            return RedirectToPage("Account/ConfirmEmailFailure");
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
            using (SmtpClient smtp = new SmtpClient(smtpSettings.Server, smtpSettings.Port))
            {
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password);

                MailMessage message = new MailMessage
                {
                    To = { to },
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(to);
                try
                {
                    await smtp.SendMailAsync(message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Email sending failed: {ex}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                }
            }
        }
       
        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}

