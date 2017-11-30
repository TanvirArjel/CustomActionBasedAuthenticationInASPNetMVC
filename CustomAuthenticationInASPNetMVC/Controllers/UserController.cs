using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CustomAuthenticationInASPNetMVC.DataAccessLayer;
using CustomAuthenticationInASPNetMVC.Models;
using CustomAuthenticationInASPNetMVC.ViewModels;
using Microsoft.AspNet.Identity;

namespace CustomAuthenticationInASPNetMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly UserDbContext _dbContext = new UserDbContext();

        // GET: Users/Create
        public async Task<ActionResult> UserRegistration()
        {
            ViewBag.AllRoles = await _dbContext.Roles.AsNoTracking().ToListAsync();
            return View();
        }

        // POST: Users/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRegistration(UserRegistrationViewModel userViewModel, List<int> selectedRoles, List<int> selectedActions)
        {
            if (ModelState.IsValid)
            {
                var isUserNameAlreadyExists = _dbContext.Users.Any(x => x.UserName == userViewModel.UserName);
                if (isUserNameAlreadyExists)
                {
                    ViewBag.UserNameExistsError = "User Name Already Exists";
                    return View(userViewModel);
                }

                var isEmailAlreadyExists = _dbContext.Users.Any(x => x.Email == userViewModel.Email);
                if (isEmailAlreadyExists)
                {
                    ViewBag.EmailExistsError = "This Email Already Registered";
                    return View(userViewModel);
                }

                var user = new User
                {
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    UserName = userViewModel.UserName,
                    Password = new PasswordHasher().HashPassword(userViewModel.Password),
                    Email = userViewModel.Email
                };

                var isRoleSuperAdminSelected = false;
                if (selectedRoles != null)
                    foreach (var selectedRole in selectedRoles)
                    {
                        var role = await _dbContext.Roles.AsNoTracking()
                            .FirstOrDefaultAsync(x => x.RoleId == selectedRole);
                        if (role != null)
                            if (string.Equals(role.RoleName, "SuperAdmin", StringComparison.CurrentCultureIgnoreCase))
                                isRoleSuperAdminSelected = true;
                    }

                if (selectedRoles != null)
                    foreach (var selectedRole in selectedRoles)
                    {
                        var role = _dbContext.Roles.Find(selectedRole);
                        user.Roles.Add(role);
                    }

                if (!isRoleSuperAdminSelected)
                {
                    

                    if (selectedActions != null)
                        foreach (var selectedAction in selectedActions)
                        {
                            var controllerAction = _dbContext.ControllerActions.Find(selectedAction);
                            user.ControllerActions.Add(controllerAction);
                        }
                }


                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("UserList");
            }
            ViewBag.AllRoles = await _dbContext.Roles.AsNoTracking().ToListAsync();
            return View(userViewModel);
        }

        [AllowAnonymous]
        public ActionResult UserLogin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> UserLogin(UserLoginViewModel userLoginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == userLoginViewModel.UserName);
                if (user == null)
                {
                    ViewBag.UserNotExistsError = "User does not exist";
                    return View(userLoginViewModel);
                }

                var passwordVerificationResult = new PasswordHasher().VerifyHashedPassword(user.Password, userLoginViewModel.Password);
                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    var userSession = new User
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        Email = user.Email
                    };
                    System.Web.HttpContext.Current.Session["LoggedInUser"] = userSession;

                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }

                ViewBag.LogInErrorMessage = "Password is Incorrect";
                return View(userLoginViewModel);
            }

            return View(userLoginViewModel);
        }

        public ActionResult LogOut()
        {
            Session.Remove("LoggedInUser");
            return RedirectToAction("UserLogin", "User");
        }


        // GET: Users
        public async Task<ActionResult> UserList()
        {
            var users = await _dbContext.Users.AsNoTracking().ToListAsync();
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<ActionResult> UserDetails(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null)
                return HttpNotFound();
            return View(user);
        }


        // GET: Users/Edit/5
        public async Task<ActionResult> UpdateUser(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var userToBeUpdated = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);
            if (userToBeUpdated == null)
                return HttpNotFound();
            ViewBag.AllRoles = await _dbContext.Roles.AsNoTracking().ToListAsync();
            return View(userToBeUpdated);
        }

        // POST: Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUser(int id, User user, List<int> selectedRoles, List<int> selectedActions)
        {
            if (ModelState.IsValid)
            {
                var userToBeUpdated = await _dbContext.Users.FindAsync(user.UserId);
                if (userToBeUpdated == null)
                    return HttpNotFound();

                if (TryUpdateModel(userToBeUpdated, "", new[] {"FirstName", "LastName", "UserName", "Email"}))
                {
                    userToBeUpdated.Roles.Clear();
              
                    userToBeUpdated.ControllerActions.Clear();


                    var isRoleSuperAdminSelected = false;
                    if (selectedRoles != null)
                        foreach (var selectedRole in selectedRoles)
                        {
                            var role = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.RoleId == selectedRole);
                            if (role != null)
                                if (string.Equals(role.RoleName, "SuperAdmin", StringComparison.CurrentCultureIgnoreCase))
                                    isRoleSuperAdminSelected = true;
                        }

                    if (selectedRoles != null)
                        foreach (var selectedRole in selectedRoles)
                        {
                            var role = await _dbContext.Roles.FindAsync(selectedRole);
                            userToBeUpdated.Roles.Add(role);
                        }

                    if (!isRoleSuperAdminSelected)
                    {
                        
                        if (selectedActions != null)
                            foreach (var selectedAction in selectedActions)
                            {
                                var controllerAction = await _dbContext.ControllerActions.FindAsync(selectedAction);
                                userToBeUpdated.ControllerActions.Add(controllerAction);
                            }
                    }

                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("UserList");
                }
                ViewBag.AllRoles = await _dbContext.Roles.AsNoTracking().ToListAsync();
                return View(user);
            }
            ViewBag.AllRoles = await _dbContext.Roles.AsNoTracking().ToListAsync();
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> DeleteUser(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var userToBeDeleted = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);
            if (userToBeDeleted == null)
                return HttpNotFound();
            return View(userToBeDeleted);
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUserConfirmed(int id)
        {
            var userToBeDeleted = await _dbContext.Users.FindAsync(id);
            if (userToBeDeleted != null)
            {
                _dbContext.Users.Remove(userToBeDeleted);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("UserList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _dbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}