using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wskh.Core;
using wskh.Data;
using wskh.Service;

namespace TimeAttendance.Web.Helper
{
    public static class UserHelper
    {
        public static string CurrentUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }
        public static wskhUser CurrentUser()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            wskhContext ctx = new wskhContext();
            var user = ctx.Users.ToList().FirstOrDefault(x => x.Id.Contains(userId));
            ctx.Dispose();
            return user;
        }

        public static string FullInformation(string userId)
        {
            var _userService = DependencyResolver.Current.GetService<IUserService>();
            string userInformation = "";
            try
            {
                var user = _userService.GetList.FirstOrDefault(x => x.Id.ToLower().Contains(userId));
                userInformation = user.FirstName != null ? user.FirstName : "";
                userInformation = user.Lastname != null ? userInformation + " " + user.Lastname : "";
            }
            catch (Exception)
            {
                userInformation = "-";
            }
            return userInformation;
        }

    }
}