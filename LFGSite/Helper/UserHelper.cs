using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoorsOpen.Models;
using DoorsOpen.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoorsOpen.Helper
{
    public class UserHelper
    {
        private SiteDbContext db;

        public UserHelper(SiteDbContext context)
        {
            db = context;
        }

        //public IEnumerable<Claim> GetClaims(string email)
        //{
        //    var claims = GetUser(email).Groups.Select(g => new Claim(ClaimTypes.Role, g.Name));
        //    return claims;
        //}

        public SecurityUser GetUser(string email)
        {
            var user = db.User.Where(u => u.Email == email).Include(u => u.Groups).FirstOrDefault();
            if(user == null)
            {
                user = new SecurityUser();
                user.Email = email;
                user.Groups = new List<SecurityGroup>();
                db.User.Add(user);
                db.SaveChanges();
            }
            var adminGroup = db.Groups.Where(g => g.Name == "Administrator").Include(g => g.Users).FirstOrDefault();
            if (adminGroup == null)
            {
                adminGroup = new SecurityGroup();
                adminGroup.Name = "Administrator";
                adminGroup.Users = new List<SecurityUser>();
                db.Groups.Add(adminGroup);
            }
            if (adminGroup.Users.Count == 0)
            {
                adminGroup.Users.Add(user);
                db.SaveChanges();
            }
            return user;
        }
    }
}
