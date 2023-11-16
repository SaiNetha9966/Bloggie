using Bloggie.Web.Models;
using Bloggie.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Permissions;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bloggie.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<Register> userManager;
        private readonly IPasswordHasher<Register> passwordHasher;
        private readonly SignInManager<Register> signInManager;
        private readonly IConfiguration config;

        public AdminController(SignInManager<Register> signInManager, UserManager<Register> userManager, IPasswordHasher<Register> passwordHash , IConfiguration config )
        {
            this.userManager = userManager;
            this.passwordHasher = passwordHash;
            this.signInManager = signInManager;
            this.config = config;
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create (User user)
        {

            var userName = await userManager.FindByNameAsync(user.Username);
            if(userName != null)
            {
                ModelState.AddModelError("", $"{userName} is alredy exist");
                return View();
            }
            
            var emailCheck = await userManager.FindByEmailAsync(user.Email);
            if(emailCheck !=null)
            {
                ModelState.AddModelError("", $"{emailCheck} is alredy exist");
                return View();
            }
            if(!ModelState.IsValid)
            {
               
                var regUser = new Register
                {
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.Username,
                    Password = user.Password        
                };


                var result = await userManager.CreateAsync (regUser,user.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                    return View();
                }
            }
            return View(user);
        }

        [HttpGet]

        public async Task<IActionResult> Read()
        {

            var data =   userManager.Users;
            if (data == null)
            {
                ModelState.AddModelError("", "No Data Found");
                return View();
            }
            else
            {
               
                return View(userManager.Users);
            }
            
            
        }

        public async Task<IActionResult> Update(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if(user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Update(Register register)
        {
            var user = await userManager.FindByIdAsync(register.Id);
            if (user != null)
            {
                if(!string.IsNullOrEmpty(register.Firstname))
                {
                    user.Firstname = register.Firstname;
                }
                else
                {
                    ModelState.AddModelError("", "FisrtName cannot be empty");
                }
                if (!string.IsNullOrEmpty(register.Lastname))
                {
                    user.Lastname = register.Lastname;
                }
                else
                {
                    ModelState.AddModelError("", "LastName cannot be empty");
                }
                if (!string.IsNullOrEmpty(register.UserName))
                {
                    user.UserName = register.UserName;
                }
                else
                {
                    ModelState.AddModelError("", "Username cannot be empty");
                }
                if (!string.IsNullOrEmpty(register.PhoneNumber))
                {
                    user.PhoneNumber = register.PhoneNumber;
                }
                else
                {
                    ModelState.AddModelError("", "Phonenumber cannot be empty");
                }
                if (!string.IsNullOrEmpty(register.Email))
                {
                    user.Email = register.Email;
                }
                else
                {
                    ModelState.AddModelError("", "Email cannot be empty");
                }
                if (!string.IsNullOrEmpty(register.Password))
                {
                    user.Password = register.Password;
                    user.PasswordHash = passwordHasher.HashPassword(user, register.Password);
                }                   
                else
                {
                    ModelState.AddModelError("", "Password cannot be empty");
                }
                  
                if ( !string.IsNullOrEmpty(register.Firstname)&& !string.IsNullOrEmpty(register.Lastname) && !string.IsNullOrEmpty(register.UserName) && !string.IsNullOrEmpty(register.PhoneNumber)&&  !string.IsNullOrEmpty(register.Email) && !string.IsNullOrEmpty(register.Password) )
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Read");
                    else
                        return RedirectToAction("Read");
                }

            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }



        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if(user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("Read");
                }
                else
                {
                    ModelState.AddModelError("", "User Not Found");
                    return RedirectToAction("Read");
                }
               
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
                return View("Read", userManager.Users);
            }
        }



        // Sign in \

        [AllowAnonymous]
        public IActionResult LogIn(string returnUrl)
        {
            LogIn login = new LogIn();
           login.ReturnUrl = returnUrl;
            return View(login);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LogIn model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username");
                return View();
            }
            var result = await signInManager.PasswordSignInAsync(model.UserName!, model.Password!, false, false);
           

            if (!ModelState.IsValid)
            {
                var token = GenerateToken(model);
                if (result.Succeeded && token != null)
                {
                   
                   
                    return RedirectToAction("Index" , "Home", token );
                }
                else if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Password");
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Invalid Credentials");
            return View(model);
        }
        private string GenerateToken(LogIn model)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var Claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.UserName),
            };

            var token = new JwtSecurityToken(
                config.GetSection("Jwt:Issuer").Value, config.GetSection("Jwt:Audience").Value,
                Claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }





        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("LogIn");
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(string name)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userData = await userManager.FindByIdAsync(userId);
            if(userData == null)
            {
                ModelState.AddModelError("", "No Data Found");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var data = new Register
                {
                    Id = userData.Id,
                    UserName = userData.UserName,
                    Firstname = userData.Firstname,
                    Lastname = userData.Lastname,
                    Email = userData.Email,
                    PhoneNumber = userData.PhoneNumber,
                    Password = userData.Password,
                };

                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetUsersDetails(string name)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userData = await userManager.FindByIdAsync(userId);    
            if(userData == null)
            {
                ModelState.AddModelError("", "No Data Found");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var data = new Register
                {
                    Id = userData.Id,
                    UserName = userData.UserName,
                    Firstname = userData.Firstname,
                    Lastname = userData.Lastname,
                    Email = userData.Email,
                    PhoneNumber = userData.PhoneNumber,
                    Password = userData.Password
                };
                return View(data);

            }
        }


        [HttpGet]

        public IActionResult getdata()
        {
            return View();
        }

    }
}
