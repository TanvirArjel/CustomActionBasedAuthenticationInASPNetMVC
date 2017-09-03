using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomAuthenticationInASPNetMVC.DataAccessLayer;
using CustomAuthenticationInASPNetMVC.Models;
using CustomAuthenticationInASPNetMVC.ViewModels;
using Microsoft.AspNet.Identity;

namespace CustomAuthenticationInASPNetMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserDbContext _dbContext = new UserDbContext();



        // GET: Users/Create
        public ActionResult UserRegistration()
        {
            ViewBag.AllRoles = _dbContext.Roles.ToList();
            return View();
        }

        // POST: Users/Create
        public JsonResult IsUserNameAlreadyExists(string UserName, int? UserId)
        {
            bool uerNameCheckingStatus = _dbContext.Users.Any(x => x.UserName == UserName && x.UserId != UserId);
            return Json(!uerNameCheckingStatus, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRegistration(UserRegistrationViewModel userViewModel, List<int> selectedRoles, List<int> selectedActionCategories,List<int> selectedActions)
        {

            if (ModelState.IsValid)
            {
                bool isUserNameAlreadyExists = _dbContext.Users.Any(x => x.UserName == userViewModel.UserName);
                if (isUserNameAlreadyExists)
                {
                    ViewBag.UserNameExistsError = "User Name Already Exists";
                    return View(userViewModel);
                }

                bool isEmailAlreadyExists = _dbContext.Users.Any(x => x.Email == userViewModel.Email);
                if (isEmailAlreadyExists)
                {
                    ViewBag.EmailExistsError = "This Email Already Registered";
                    return View(userViewModel);
                }

                User user = new User()
                {
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    UserName =  userViewModel.UserName,
                    Password = new PasswordHasher().HashPassword(userViewModel.Password),
                    Email = userViewModel.Email
                };

                if (selectedRoles != null)
                {
                    foreach (var selectedRole in selectedRoles)
                    {
                        Role role = _dbContext.Roles.Find(selectedRole);
                        user.Roles.Add(role);
                    }
                }

                if (selectedActionCategories != null)
                {
                    foreach (var selectedActionCategory in selectedActionCategories)
                    {
                        ActionCategory actionCategory = _dbContext.ActionCategories.Find(selectedActionCategory);
                        user.ActionCategories.Add(actionCategory);
                    }
                }

                if (selectedActions != null)
                {
                    foreach (var selectedAction in selectedActions)
                    {
                        ControllerAction controllerAction = _dbContext.ControllerActions.Find(selectedAction);
                        user.ControllerActions.Add(controllerAction);
                    }
                }

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

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
                User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userLoginViewModel.UserName);
                if (user == null)
                {
                    ViewBag.UserNotExistsError = "User does not exist";
                    return View(userLoginViewModel);
                }

                var passwordVerificationResult = new PasswordHasher().VerifyHashedPassword(user.Password, userLoginViewModel.Password);
                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    User userSession = new User()
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        Email = user.Email
                    };
                    System.Web.HttpContext.Current.Session["LoggedInUser"] = userSession;

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");

                }
      
                ViewBag.LogInErrorMessage = "User Name or Password is Incorrect";
                return View();   
            }

            return View(userLoginViewModel);
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        // GET: Users
        public async Task<ActionResult> Index()
        {
            return View(await _dbContext.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        
        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.AllRoles = _dbContext.Roles.ToList();
            return View(user);
        }

        // POST: Users/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(User user, List<int> selectedRoles, List<int> selectedActionCategories, List<int> selectedActions)
        {
            if (ModelState.IsValid)
            {
                User userToBeUpdated = _dbContext.Users.Find(user.UserId);
                if (userToBeUpdated == null)
                {
                    return HttpNotFound();
                }
                userToBeUpdated.FirstName = user.FirstName;
                userToBeUpdated.LastName = user.LastName;
                userToBeUpdated.UserName = user.UserName;
                userToBeUpdated.Email = user.Email;
                userToBeUpdated.Roles.Clear();
                userToBeUpdated.ActionCategories.Clear();
                userToBeUpdated.ControllerActions.Clear();

                if (selectedRoles != null)
                {
                    foreach (var selectedRole in selectedRoles)
                    {
                        Role role = _dbContext.Roles.Find(selectedRole);
                        userToBeUpdated.Roles.Add(role);
                    }
                }

                if (selectedActionCategories != null)
                {
                    foreach (var selectedActionCategory in selectedActionCategories)
                    {
                        ActionCategory actionCategory = _dbContext.ActionCategories.Find(selectedActionCategory);
                        userToBeUpdated.ActionCategories.Add(actionCategory);
                    }
                }

                if (selectedActions != null)
                {
                    foreach (var selectedAction in selectedActions)
                    {
                        ControllerAction controllerAction = _dbContext.ControllerActions.Find(selectedAction);
                        userToBeUpdated.ControllerActions.Add(controllerAction);
                    }
                }
                _dbContext.Entry(userToBeUpdated).State = EntityState.Modified;
                
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await _dbContext.Users.FindAsync(id);
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
