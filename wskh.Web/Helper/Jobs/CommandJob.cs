using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TimeAttendance.Web.Helper.Jobs
{
    [DisallowConcurrentExecution]
    public class CommandJob : IJob
    {
        public CommandJob()
        {

        }
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                CommandEntityHelper.Analyze();
            }
            catch (Exception e)
            {
            }

            return Task.CompletedTask;
        }
    }
}