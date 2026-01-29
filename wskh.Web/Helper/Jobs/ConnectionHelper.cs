using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace wskh.Web.Helper.Jobs
{
    public static class ConnectionHelper
    {
        public static string Get()
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["wskhContext"];
            return mySetting.ConnectionString;
        }
    }
}