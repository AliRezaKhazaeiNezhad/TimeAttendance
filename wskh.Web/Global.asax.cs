using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TimeAttendance.Web.Helper.Jobs;
using wskh.FingerTec;
using wskh.Web.Helper;
using wskh.Web.Helper.Jobs;

namespace wskh.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperHelper.Initialize();


            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.Start();


            //#region جاب یک برای آنالیز فرمان ها
            //IJobDetail jobZero = JobBuilder.Create<CommandJob>().Build();

            //ITrigger triggerZero = TriggerBuilder.Create()
            //    .WithIdentity("CommandTrigger", "CommandGroup")
            //    .StartAt(DateTime.Now.AddSeconds(50))
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInSeconds(20)
            //        .RepeatForever())
            //    .Build();

            //scheduler.ScheduleJob(jobZero, triggerZero);
            //#endregion


            //#region جاب یک برای ایجاد گزارش روزانه
            //IJobDetail jobReportDay = JobBuilder.Create<ReportDayJob>().Build();

            //ITrigger triggerReportDay = TriggerBuilder.Create()
            //    .WithIdentity("ReportDayTrigger", "ReportDayGroup")
            //    .StartAt(DateTime.Now.AddSeconds(1))
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInMinutes(1)
            //        .RepeatForever())
            //    .Build();

            //scheduler.ScheduleJob(jobReportDay, triggerReportDay);
            //#endregion


            //#region جاب برای روزهای بدون لاگ مثل جمعه میاید تولید گزارش کرده و گزارشات روزانه تکراری را حذف میکند
            //IJobDetail reportDayHelper = JobBuilder.Create<ReportDayDuplicatorJob>().Build();

            //ITrigger triggerReportDayHelper = TriggerBuilder.Create()
            //    .WithIdentity("triggerReportDayHelperTrigger", "triggerReportDayHelperGroup")
            //    .StartAt(DateTime.Now.AddSeconds(5))
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInMinutes(7)
            //        .RepeatForever())
            //    .Build();

            //scheduler.ScheduleJob(reportDayHelper, triggerReportDayHelper);
            //#endregion


            //#region جاب برای گزارش تحلیل شده
            //IJobDetail analyzedReportHelper = JobBuilder.Create<AnalyzedReportJob>().Build();

            //ITrigger triggerAnalyzedReportHelper = TriggerBuilder.Create()
            //    .WithIdentity("triggerAnalyzedReportHelperTrigger", "triggerAnalyzedReportGroup")
            //   .StartNow()
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInMinutes(61)
            //        .RepeatForever())
            //    .Build();

            //scheduler.ScheduleJob(analyzedReportHelper, triggerAnalyzedReportHelper);
            //#endregion

        }
    }
}
