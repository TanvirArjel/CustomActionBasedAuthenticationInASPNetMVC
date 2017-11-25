using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CustomAuthenticationInASPNetMVC.CommonCode;
using CustomAuthenticationInASPNetMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using CustomAuthenticationInASPNetMVC.DataAccessLayer;
using CustomAuthenticationInASPNetMVC.ViewModels;

namespace CustomAuthenticationInASPNetMVC.Controllers
{
    public class ManageController : Controller
    {
        private readonly UserDbContext _dbContext = new UserDbContext();

        private readonly User _loggedInUser = System.Web.HttpContext.Current.Session["LoggedInUser"] as User;

        private readonly PasswordHasher _passWordHasher = new PasswordHasher();
        
        // GET: Manage

        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _dbContext.Users.FindAsync(_loggedInUser.UserId);
                if (user == null)
                {
                    return RedirectToAction("UserLogin", "User");
                }
                PasswordVerificationResult passwordVerificationResult = _passWordHasher.VerifyHashedPassword(user.Password, model.OldPassword);
                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    ViewBag.PasswordError = "Old Password is not correct";
                    return View(model);
                }

                var newHashedPassword = _passWordHasher.HashPassword(model.NewPassword);
                user.Password = newHashedPassword;
                _dbContext.Entry(user).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                ViewBag.PasswordChangeSuccessMessage = "Password has been changed successfully";
                return RedirectToAction("UserLogin", "User");
            }
            
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _dbContext.Users.Where(x => x.Email == model.Email).FirstOrDefaultAsync();
                if (user == null)
                {
                    ViewBag.ErrorrMessage = "No user found with the provided email address";
                    return View("ForgotPassword");
                }

                var passwordResetToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
                
                
                //Code for Sending an email with this link
                var callbackUrl = Url.Action("ResetPassword", "Manage", new { userId = user.UserId, token = passwordResetToken }, protocol: Request.Url.Scheme);

                var mailFrom = "tanvirdu7@gmail.com";
                var mailTo = model.Email; // To which you want to send the mail
                using (MailMessage mail = new MailMessage(mailFrom, mailTo))
                {
                    mail.From = new MailAddress(mailFrom);
                    mail.Subject = "Password Reset";
                    mail.Body = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";
                    mail.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        //Smtp configuration.You can also do it in web.config file
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential networkCredential = new NetworkCredential("tanvirdu7@gmail.com", "password");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = networkCredential;
                        smtp.Port = 587; //This for gmail

                        smtp.Send(mail);

                        bool passwordResetTokenInsertStatus = await new PasswordReset().InsertPasswordResetToken(user.UserId, passwordResetToken);
                        if (!passwordResetTokenInsertStatus)
                        {
                            ViewBag.ErrorrMessage = "There is some problem in the server.Please try again";
                            return View("ForgotPassword");
                        }
                    }

                }

                
                return RedirectToAction("ForgotPasswordConfirmation", "Manage");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(int userId, string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return View("Error");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int userId = Convert.ToInt32(model.UserId);
            bool isPasswordResetPermitted = await new PasswordReset().IsPasswordResetPermitted(userId, model.Code);
            if (!isPasswordResetPermitted)
            {
                ViewBag.PasswordResetPermitionError = "You can not reset password with this token! May be the token is expired or is reset alreay!";
                return View();
            }
            User user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Manage");
            }

            user.Password = _passWordHasher.HashPassword(model.NewPassword);
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("ResetPasswordConfirmation", "Manage");
            
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

    }
}