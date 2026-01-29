using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Core.Enumerator;
using wskh.Service;

namespace TimeAttendance.WebEssentials.CommandPart
{
    public static class CommandHelper
    {

        /// <summary>
        /// ایجاد فرمان
        /// </summary>
        /// <param name="category"></param>
        /// <param name="objectIntId"></param>
        /// <param name="objectStringId"></param>
        /// <param name="title"></param>
        /// <param name="count"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static int Create(CommandCategory category, bool analyzed, string title, int count, wskhUser user, int entityId = 0)
        {
            var commandService = DependencyResolver.Current.GetService<ICommandService>();
            int result = 0;
            Command cmd = new Command();
            try
            {
                cmd.CommandCategory = category;
                cmd.State = analyzed ? CommandState.Analyzed : CommandState.Pending;
                cmd.Title = title;
                cmd.Count = count;
                cmd.UserId = user.Id;
                cmd.EntityId = entityId;
                cmd.CreateDateTime = DateTime.Now;
                cmd.State = CommandState.Pending;


                if (analyzed)
                {
                    cmd.StartingDateTime = cmd.CreateDateTime;
                    cmd.FinishDateTime = cmd.CreateDateTime;
                    cmd.State = CommandState.Analyzed;
                }

                commandService.Create(cmd);
                result = cmd.Id;
            }
            catch (Exception e)
            {
                result = 0;
            }
            commandService.Dispose();
            return result;
        }
    }
}
