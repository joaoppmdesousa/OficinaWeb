using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OficinaWeb.Data;
using OficinaWeb.Data.Entities;
using OficinaWeb.Helpers;
using OficinaWeb.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OficinaWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IEmailHelper _emailHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IConfiguration _configuration;
        private readonly IClientRepository _clientRepository;
        private readonly ICloudinaryHelper _cloudinaryHelper;

        public AccountController(
            IUserHelper userHelper,
            IEmailHelper emailHelper,
            IConverterHelper converterHelper,
            IConfiguration configuration,
            IClientRepository clientRepository,
            ICloudinaryHelper cloudinaryHelper)
        {
            _userHelper = userHelper;
            _emailHelper = emailHelper;
            _converterHelper = converterHelper;
            _configuration = configuration;
            _clientRepository = clientRepository;
            _cloudinaryHelper = cloudinaryHelper;
        }


        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }


            this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }



        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {

            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Admin", Value = "Admin" },
                new SelectListItem { Text = "Employee", Value = "Employee" }
            };

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {

            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Admin", Value = "Admin" },
                new SelectListItem { Text = "Employee", Value = "Employee" },
            };

            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    user = new User
                    {
                        Name = model.Name,
                        Email = model.Username,
                        UserName = model.Username
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to create user.");
                        return View(model);
                    }


                    await _userHelper.AddUserToRoleAsync(user, model.Role);


                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userId = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    Response response = _emailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                        $"To allow the user, " +
                        $"please click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                    if (!response.IsSuccess)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to send confirmation email.");
                    }

                    if (response.IsSuccess)
                    {
                        TempData["Message"] = "The instructions to confirm your account have been sent to the email provided.";
                        return RedirectToAction("RegisterConfirmation");
                    }


                }
            }

            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Admin", Value = "Admin" },
                new SelectListItem { Text = "Employee", Value = "Employee" }
            };

            return RedirectToAction("AdminClientList", "Clients"); ;
        }


        public IActionResult RegisterConfirmation()
        {
            return View();
        }



            public async Task<IActionResult> ChangeUser()
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                var model = new ChangeUserViewModel();
                if (user != null)
                {
                    model.Name = user.Name;
                    model.ImageUrl = user.ImageUrl;
                }

                return View(model);
            }


            [HttpPost]
            public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
            {
                if (ModelState.IsValid)
                {


                    var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    if (user != null)
                    {
                        if (model.ImageFile != null && model.ImageFile.Length > 0)
                        {
                            var url = await _cloudinaryHelper.UploadImageAsync(model.ImageFile);
                            user.ImageUrl = url;
                        }


                        user.Name = model.Name;
                        var response = await _userHelper.UpdateUserAsync(user);
                        if (response.Succeeded)
                        {
                            ViewBag.UserMessage = "User updated successfully.";
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                        }

                    }
                }


                return View(model);

            }


        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return this.View(model);
        }



        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {

                    //if (await _userHelper.IsUserInRoleAsync(user, "Client"))
                    //{

                    //    bool isDefaultPassword = await _userHelper.CheckPasswordAsync(user, "defaultpassclient");
                    //    if (isDefaultPassword)
                    //    {
                    //        return BadRequest("You must set your password before logging in.");
                    //    }
                    //}

                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);

                    }
                }
            }

            return BadRequest();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }


        public async Task<IActionResult> SetPassword(string userId, string token)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            var model = new SetPasswordViewModel { UserId = userId };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByIdAsync(model.UserId);
                if (user != null)
                {
                    var result = await _userHelper.SetPasswordAsync(user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("Login");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return this.View(model);
        }


        public IActionResult RecoverPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _emailHelper.SendEmail(model.Email, "Password Reset", $"<h1> Password Reset</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                }

                return this.View();

            }

            return this.View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }





        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUserForClient(int clientId)
        {
            var client = await _clientRepository.GetIdAsync(clientId);
            if (client == null) return NotFound();

            var existingUser = await _userHelper.GetUserByEmailAsync(client.Email);
            if (existingUser != null)
            {
                TempData["Error"] = "There is already a client with that email";
                return RedirectToAction("AdminClientList", "Clients");
            }

            var user = new User
            {
                Name = client.Name,
                Email = client.Email,
                UserName = client.Email,
                PhoneNumber = client.Contact,
            };

            var result = await _userHelper.AddUserAsync(user, "defaultpassclient");

            if (result.Succeeded)
            {
                await _userHelper.AddUserToRoleAsync(user, "Client");

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                var model = _converterHelper.ToRegisterNewUserViewModel(user, "Client");

                string tokenLink = Url.Action("SetPassword", "Account", new
                {
                    userId = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _emailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"you will need to set your password,please click in this link:</br></br><a href = \"{tokenLink}\">Set Password</a>");


                if (!response.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, "Failed to send confirmation email.");
                }

                if (response.IsSuccess)
                {
                    TempData["Message"] = "The instructions to confirm your account have been sent to the email provided.";
                    return RedirectToAction("RegisterConfirmation");
                }
               
            }
            else
            {
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction("AdminClientList", "Clients"); ;
        }



        public IActionResult NotAuthorized()
        {
            return View();
        }




    
    }
}
