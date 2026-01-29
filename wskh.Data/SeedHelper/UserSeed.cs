using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;
using wskh.Data;

namespace TimeAttendance.Data.SeedHelper
{
    public static class UserSeed
    {
        public static void Initial()
        {
            wskhContext context = new wskhContext();
            var adminHash = HashHelper.Encrypt("admin");
            var adminForBartarHash = HashHelper.Encrypt("bartaradmin");
            #region Users
            if (!context.Users.Any(r => r.UserName == adminForBartarHash))
            {
                var store = new UserStore<wskhUser>(context);
                var manager = new UserManager<wskhUser>(store);
                var user = new wskhUser
                {
                    UserName = adminForBartarHash,
                    UserRoleType = adminHash,
                    Active = true
                };


                manager.Create(user, "Bartar@dmin1234567");
            }

            if (!context.Users.Any(r => r.UserName == adminHash))
            {
                var store = new UserStore<wskhUser>(context);
                var manager = new UserManager<wskhUser>(store);
                var user = new wskhUser
                {
                    UserName = adminHash,
                    UserRoleType = adminHash,
                    Active = true
                };


                manager.Create(user, "@dmin01235");
            }

            var demoHash = HashHelper.Encrypt("demo");

            if (!context.Users.Any(r => r.UserName == demoHash))
            {
                var store = new UserStore<wskhUser>(context);
                var manager = new UserManager<wskhUser>(store);
                var user = new wskhUser
                {
                    UserName = demoHash,
                    UserRoleType = adminHash,
                    Active = true
                };
                manager.Create(user, "demo123456");
            }
            #endregion
            #region UserGrouping
            if (context.UserGroups == null || context.UserGroups.Count() <= 0)
            {
                context.UserGroups.Add(new Core.UserGroup() {
                    Title = "گروه پیش فرض",
                    Remove = false
                });
                context.SaveChanges();
            }
            #endregion
        }
    }
}
