using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomAuthenticationInASPNetMVC.DataAccessLayer;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace CustomAuthenticationInASPNetMVC.Repository
{
    public class UserRepository
    {
        private readonly UserDbContext _dbContext = new UserDbContext();
        public bool IsUserInRole(string userName, string roleNames)
        {
            bool isUserInRole = true;
            if (roleNames != null)
            {
                var roles = roleNames.Split();
                foreach (var role in roles)
                {
                    isUserInRole = _dbContext.Users.Where(x => x.UserName == userName).Any(x => x.Roles.Any(r => r.RoleName == role));
                }
            }
            

            
            return isUserInRole;
        }
    }
}