using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Web.Helper;
using TimeAttendance.WebEssentials;
using TimeAttendance.WebEssentials.ReportPart;
using wskh.FingerTec;
using wskh.LogAndEnrlol.analyzer.SQL;
using wskh.Service;

namespace wskh.Web.Helper.Jobs
{
    [DisallowConcurrentExecution]
    public class ReportDayDuplicatorJob : IJob
    {
        public ReportDayDuplicatorJob()
        {

        }
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                ReportDayDuplicatorHelper.Initial();
            }
            catch (Exception e)
            {
            }

            return Task.CompletedTask;
        }
    }
}