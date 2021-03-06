﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lend_er.Data;
using Lend_er.Services.Services;
using Lend_er.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lend_er.Web.Controllers
{
    //[Route("Account")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //[Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //[Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.photo != null)
                {
                    string uploadfolder = Path.Combine(webHostEnvironment.WebRootPath, "ProfilePhoto");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.photo.FileName;
                    string filepath = Path.Combine(uploadfolder, uniqueFileName);
                    model.photo.CopyTo(new FileStream(filepath, FileMode.Create));
                }
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    Gender = model.Gender,
                    State = model.State,
                    PhotoPath = uniqueFileName,
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                    string folder = Path.Combine(webHostEnvironment.WebRootPath, "EmailTokens");
                    string filename = "EmailToken_" + model.Email + "_" + DateTime.Today.Day + "_" + DateTime.Today.Month + "_" + DateTime.Today.Year + ".txt";
                    //log to file
                    MyLogger myLogger = new MyLogger(folder, filename);
                    myLogger.LogToFile(confirmationUrl);
                    return View("RegistrationSuccessful");
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        //[Route("")]
        //[Route("login")]
        //[Route("~/")]
        [HttpGet]
        public IActionResult Login()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("index", "Home");
            }
            return View();
        }

        //[Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //find the user
                var user = await userManager.FindByEmailAsync(model.Email);
                //block user if email is not confirmed
                if (user != null && user.EmailConfirmed.Equals(false) && (await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError("", "Email not confirmed");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: model.RememberMe, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "Home");
                }
                if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            return View(model);
        }

        //[Route("Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    ViewBag.ErrorTitle = "User Not Found!";
                    ViewBag.ErrorMessage = $"The user with ID = {userId} was not found";
                    return View("Error");
                }
                else
                {
                    var result = await userManager.ConfirmEmailAsync(user, token);
                    if (result.Succeeded)
                    {
                        return View();
                    }
                    ViewBag.ErrorTitle = "Email Confirmation Failed";
                    ViewBag.ErrorMessage = "we faild to confirm the email address. please contact the administrators.";
                    return View("Error");
                }
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && (await userManager.IsEmailConfirmedAsync(user)))
                {
                    //generate password reset token
                    string token = await userManager.GeneratePasswordResetTokenAsync(user);
                    //generate the link
                    string confirmationLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token },Request.Scheme);
                    string folder = Path.Combine(webHostEnvironment.WebRootPath, "PasswordReset");
                    string filename = "ResetPassword_" + model.Email + "_" + DateTime.Today.Day + "_" + DateTime.Today.Month + "_" + DateTime.Today.Year + ".txt";
                    //log it
                    var log = new MyLogger(folder, filename);
                    log.LogToFile(confirmationLink);
                    ViewBag.ErrorTitle = "Password Reset Link";
                    ViewBag.ErrorMessage = "We have emailed you a link to reset your password";
                    return View("Error");
                }
                ViewBag.ErrorTitle = "Not Found!";
                ViewBag.ErrorMessage = "Sorry your account was not found or your email has not been confirmed";
                return View("Error");
            }
            ModelState.AddModelError("", "Check email");
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var model = new PasswordResetViewModel()
            {
                Email = email,
                Token = token,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        if (await userManager.IsLockedOutAsync(user))
                        {
                            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);
                        }
                        return View("PasswordReset");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                ViewBag.ErrorTitle = "Not Found";
                ViewBag.ErrorMessage = $"the user with email {model.Email} was not found";
                return View("Error");
            }
            ModelState.AddModelError("", "Invalid Password");
            return View(model);
        }
    }
}
