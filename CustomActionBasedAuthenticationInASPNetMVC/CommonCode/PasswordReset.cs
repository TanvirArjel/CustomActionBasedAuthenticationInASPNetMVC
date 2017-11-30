using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CustomActionBasedAuthenticationInASPNetMVC.DataAccessLayer;
using CustomActionBasedAuthenticationInASPNetMVC.Models;


namespace CustomActionBasedAuthenticationInASPNetMVC.CommonCode
{
    public class PasswordReset : IDisposable
    {
        private readonly UserDbContext _dbContext = new UserDbContext();
       
        public async Task<bool> IsPasswordResetPermitted(int userId, string passwordResetToken)
        {
            DateTime validTimeLimit = DateTime.Now.AddMinutes(-10);
            var userPasswordReset = await _dbContext.UserPasswordResets.FirstOrDefaultAsync(x => x.CreatedOn > validTimeLimit && x.UserId == userId &&
                            x.PasswordResetToken == passwordResetToken  && x.IsReset == false);
            if (userPasswordReset != null)
            {
                userPasswordReset.IsReset = true;
                _dbContext.Entry(userPasswordReset).State=EntityState.Modified;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        
        public async Task<bool> InsertPasswordResetToken(int userId,string passwordResetToken)
        {
            UserPasswordReset userPasswordReset = new UserPasswordReset()
            {
                UserId = userId,
                PasswordResetToken = passwordResetToken,
                CreatedOn = DateTime.Now,
                IsReset = false,
            };
            _dbContext.UserPasswordResets.Add(userPasswordReset);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        //public string PasswordResetBody(string Code)
        //{
        //    string Body = "";
        //    StreamReader sr=new StreamReader(HttpContext.Current.Server.MapPath("~/Template/EmailTemplate/PasswordResetFirst.html"));
        //    Body += sr.ReadToEnd();
        //    Body += "<span target='_blank' href='#' class='link2' style='color:#ffffff;'>"+Code+"</span>";
        //    sr = new StreamReader(HttpContext.Current.Server.MapPath("~/Template/EmailTemplate/PasswordResetLast.html"));
        //    Body += sr.ReadToEnd();
        //    return Body;

        //}


        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            //dispose unmanaged resources
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PasswordReset()
        {
            Dispose(false);
        }
    }
}