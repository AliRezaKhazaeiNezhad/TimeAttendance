using System.Web.Mvc;

namespace wskh.Web.Areas.TimeAttendance
{
    public class TimeAttendanceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TimeAttendance";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TimeAttendance_default",
                "TimeAttendance/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}