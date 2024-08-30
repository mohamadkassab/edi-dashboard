using EDI.Models.UserModels;
using EDI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using EDI.Utilities.UserUtilities;
using EDI.ViewModels.UserViewModels;
using EDI.Services.DatabaseServices;
using NuGet.Protocol.Plugins;
using System.Transactions;

namespace EDI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly DatabaseConnection _connection;
        private readonly Repository _repository;
        private readonly ErrorController _errorController;
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new DatabaseConnection(configuration);
            _errorController = new ErrorController(configuration);
            _repository = new Repository(configuration);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            ViewData["Title"] = "Login EDI";
            return View("Login"); 
        }
   
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(UserLoginViewModel userLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(userLogin);
                }
                else
                {
                    using (SqlConnection connection = await _connection.OpenConnectionAsync())
                    {
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            UserModel queryUser = new UserModel();
                            queryUser.Email = userLogin.Email.ToLower().Trim();
                            string query = _configuration?.GetSection("StoredProcedures:EDIA_User_check").Value;

                            List<UserModel> users = await _repository.DBExcuteReader<UserModel>(connection, transaction, query, queryUser);
                            if (users.Count >=1 && users[0].IsActive == true )
                            {
                                queryUser = users[0];
                                if (BCrypt.Net.BCrypt.Verify(userLogin.Password, queryUser.Password))
                                {                                   
                                    UserRolePermissionModel userRolePermission = new UserRolePermissionModel();
                                    userRolePermission.Id = (int)queryUser.RoleId;

                                    query = _configuration?.GetSection("StoredProcedures:EDIA_UserRolePermission_Get").Value;
                                    List<UserRolePermissionModel> usersRolePermission = await _repository.DBExcuteReader<UserRolePermissionModel>(connection, transaction, query, userRolePermission);

                                    if (usersRolePermission.Count >= 1) 
                                    {
                                        List<Claim> claims = new List<Claim>
                                        {
                                            new Claim(ClaimTypes.Name, queryUser.FirstName),
                                            new Claim(ClaimTypes.Email, queryUser.Email),
                                            new Claim(ClaimTypes.Role, usersRolePermission[0].Role),
                                            new Claim("CanCreateUser", usersRolePermission[0].CanCreateUser.ToString()),
                                            new Claim("CanApproveContent", usersRolePermission[0].CanApproveContent.ToString())
                                        };

                                        Jwt jwtAuth = new Jwt(_configuration);
                                        string jwt = (await jwtAuth.JwtGenerator(claims, connection))!;
                                        var cookieOptions = new CookieOptions
                                        {
                                            HttpOnly = true,
                                            Expires = DateTime.Now.AddHours(8) 
                                        };

                                        HttpContext.Response.Cookies.Append("ediAuth", jwt, cookieOptions);
                                        HttpContext.Session.SetString("UserRole", usersRolePermission[0].Role);
                                        HttpContext.Session.SetString("UserEmail", queryUser.Email);
                                        HttpContext.Session.SetString("UserFirstName", queryUser.FirstName);
                                        HttpContext.Session.SetString("UserLastName", queryUser.LastName);
                                        HttpContext.Session.SetString("CanCreateUser", usersRolePermission[0].CanCreateUser.ToString());
                                        HttpContext.Session.SetString("CanApproveContent", usersRolePermission[0].CanApproveContent.ToString());
                                        return RedirectToAction("Main", "Home");
                                    }
                                }
                            }
                            
                            TempData["login-failed"] = true;
                            return View();                         
                        }    
                    }
                }
            }
            catch (Exception ex)
            {
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string FuntionName = ControllerContext.ActionDescriptor.DisplayName!;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                TempData["login-failed"] = true;
                return View();
            }
        }

        [Authorize(Policy = "CanCreateUser")]
        public async Task<IActionResult> Register()
        {
            List<UserRoleViewModel> userRoles = new List<UserRoleViewModel>();

            using (SqlConnection connection = await _connection.OpenConnectionAsync())
            {
                using (SqlTransaction transaction = connection.BeginTransaction())
                {

                    string query = _configuration?.GetSection("StoredProcedures:EDIA_GetRoles").Value;
                    userRoles = await _repository.DBExcuteReader<UserRoleViewModel>(connection, transaction, query, null);
                }
            }

            ViewData["Title"] = "Registration EDI";
            ViewData["UserRoles"] = userRoles;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanCreateUser")]
        public async Task<IActionResult> Register(UserModel user)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(user);
                }
                else
                {                 
                    using (SqlConnection connection = await _connection.OpenConnectionAsync())
                    {
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            string query;
                            query = _configuration?.GetSection("StoredProcedures:EDIA_User_check").Value;
                            UserModel queryUser = new UserModel();
                            queryUser.Email = user.Email.ToLower().Trim();

                            List<UserModel> emailExist = await _repository.DBExcuteReader<UserModel>(connection, transaction, query, queryUser);

                            if (emailExist.Count > 0)
                            {
                                TempData["email-exist"] = true;
                                ModelState.AddModelError("Email", "Email already exist!");
                                return View(user);
                            }
                            else
                            {
                                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, BCrypt.Net.BCrypt.GenerateSalt(12));
                                user.FirstName = user.FirstName.ToLower().Trim();
                                user.LastName = user.LastName.ToLower().Trim();
                                user.Email = user.Email.ToLower().Trim();
                                user.IsActive = true;
                                query = _configuration?.GetSection("StoredProcedures:EDIA_User_Register").Value;
                                bool result = await _repository.DBExcuteNonQuery(connection, transaction, query, user);
                                if (result)
                                {
                                    transaction.Commit();
                                    TempData["success"] = true;
                                    return RedirectToAction("Register", "User");

                                }
                            }           
                        }
                    } 
                }
                TempData["error"] = true;
                return View(); 
            }
            catch(Exception ex)
            {
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string FuntionName = ControllerContext.ActionDescriptor.DisplayName!;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                TempData["error"] = true;
                return View();
            }  
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                HttpContext.Session.Remove("UserRole");
                HttpContext.Session.Remove("UserEmail");
                HttpContext.Response.Cookies.Delete("ediAuth");
                return RedirectToAction("Login", "User");
            }
            catch(Exception ex)
            {
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string FuntionName = ControllerContext.ActionDescriptor.DisplayName!;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                TempData["error"] = true;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            ViewData["Title"] = "Change password EDI";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserChangePasswordViewModel userChangePassword)
        {
            try
            {
                if(!ModelState.IsValid || userChangePassword.NewPassword != userChangePassword.ConfirmNewPassword)
                {
                    TempData["error"] = true;
                    return View(userChangePassword);
                }
                else
                {
                    using (SqlConnection connection = await _connection.OpenConnectionAsync())
                    {
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            string query = _configuration?.GetSection("StoredProcedures:EDIA_User_check").Value;
                            UserModel queryUser = new UserModel();
                            queryUser.Email = HttpContext?.Session?.GetString("UserEmail")!;
                            List<UserModel> emailExist = await _repository.DBExcuteReader<UserModel>(connection, transaction, query, queryUser);

                            if (emailExist.Count > 0 && BCrypt.Net.BCrypt.Verify(userChangePassword.OldPassword, emailExist[0].Password))
                            {
                                queryUser.Password = BCrypt.Net.BCrypt.HashPassword(userChangePassword.NewPassword, BCrypt.Net.BCrypt.GenerateSalt(12));

                                query = _configuration?.GetSection("StoredProcedures:EDIA_User_ChangePassword").Value;
                                bool result = await _repository.DBExcuteNonQuery(connection, transaction, query, queryUser);

                                if (result)
                                {
                                    transaction.Commit();
                                    return RedirectToAction("Logout", "User");
                                }
                            }

                            TempData["error"] = true;
                            return View();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string FuntionName = ControllerContext.ActionDescriptor.DisplayName!;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                TempData["error"] = true;
                return View();
            }
        }
    }
}
