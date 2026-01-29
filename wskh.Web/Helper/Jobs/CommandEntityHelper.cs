using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.WebEssentials;
using wskh.Core.Enumerator;
using wskh.LogAndEnrlol.analyzer.SQL;
using wskh.Service;

namespace TimeAttendance.Web.Helper.Jobs
{
    public static class CommandEntityHelper
    {
        public static async void Analyze()
        {
            var _commandService = DependencyResolver.Current.GetService<ICommandService>();

            var list = _commandService.GetList;

            _commandService.Dispose();


            try
            {
                if (list != null)
                {
                    list = list.Where(x => x.State == CommandState.Pending).ToList();


                    var logCommandList = list.Where(x => x.CommandCategory == CommandCategory.LogCommand).ToList();

                    var enrollCommandList = list.Where(x => x.CommandCategory == CommandCategory.EnrollCommand).ToList();

                    var deviceList = list.Where(x => x.CommandCategory == CommandCategory.DeviceCommand).ToList();


                    var calendarAddedList = list.Where(x => x.CommandCategory == CommandCategory.CalendarAdded).ToList();

                    //var calendarUpdateList = list.Where(x => x.CommandCategory == CommandCategory.CalendarUpdate).ToList();


                    var specialDayList = list.Where(x => x.CommandCategory == CommandCategory.SpecialDayChange).ToList();

                    var enrllDeAssign = list.Where(x => x.CommandCategory == CommandCategory.RemoveEnrollFromUser).ToList();

                    var enrllAssign = list.Where(x => x.CommandCategory == CommandCategory.AssignEnrollToUser).ToList();






                    bool logResult = await LogAnalyzer(logCommandList);

                    bool enrollResult = await EnrollAnalyzer(enrollCommandList);

                    bool deviceResult = await DeviceAnalyzer(deviceList);

                    bool addCalendarResult = await CalendarAddOrUpdateAnalyzer(calendarAddedList);

                    //bool updateCalendarResult = await CalendarAddOrUpdateAnalyzer(calendarUpdateList);

                    bool specialDayResult = await SpecialDayAnalyzer(specialDayList);

                    bool enrllDeAssignResult = await EnrollDeAssignAnalyzer(enrllDeAssign);

                    bool enrllAssignResult = await EnrollAssignAnalyzer(enrllAssign);
                }
            }
            catch (Exception e)
            {

            }
        }



        private static async Task<bool> LogAnalyzer(List<Command> commands)
        {
            bool result = false;


            if (commands != null && commands.Count() > 0)
            {
                foreach (var cmd in commands)
                {
                    SQLRepository _sqlRepository = new SQLRepository(ConfigurationManager.ConnectionStrings["wskhContext"].ConnectionString);

                    var _commandService = DependencyResolver.Current.GetService<ICommandService>();

                    var findCommand = _commandService.FindById(cmd.Id);



                    try
                    {

                        findCommand.State = CommandState.Analyzing;

                        _commandService.Update(findCommand);

                        findCommand.StartingDateTime = DateTime.Now;

                        _sqlRepository.LogPreAnalyze();

                        findCommand.State = CommandState.Analyzed;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }
                    catch (Exception e)
                    {
                        findCommand.State = CommandState.Fraction;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }

                }
            }


            return result;
        }

        private static async Task<bool> EnrollAnalyzer(List<Command> commands)
        {
            bool result = false;


            if (commands != null && commands.Count() > 0)
            {
                foreach (var cmd in commands)
                {
                    SQLRepository _sqlRepository = new SQLRepository(ConfigurationManager.ConnectionStrings["wskhContext"].ConnectionString);

                    var _commandService = DependencyResolver.Current.GetService<ICommandService>();

                    var findCommand = _commandService.FindById(cmd.Id);



                    try
                    {
                        findCommand.State = CommandState.Analyzing;

                        _commandService.Update(findCommand);

                        findCommand.StartingDateTime = DateTime.Now;

                        _sqlRepository.EnrollAnalyze();

                        findCommand.State = CommandState.Analyzed;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }
                    catch (Exception e)
                    {
                        findCommand.State = CommandState.Fraction;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }

                }
            }


            return result;
        }

        private static async Task<bool> DeviceAnalyzer(List<Command> commands)
        {
            bool result = false;


            if (commands != null && commands.Count() > 0)
            {
                foreach (var cmd in commands)
                {
                    SQLRepository _sqlRepository = new SQLRepository(ConfigurationManager.ConnectionStrings["wskhContext"].ConnectionString);

                    var _commandService = DependencyResolver.Current.GetService<ICommandService>();

                    var findCommand = _commandService.FindById(cmd.Id);



                    try
                    {

                        findCommand.State = CommandState.Analyzing;

                        _commandService.Update(findCommand);

                        findCommand.StartingDateTime = DateTime.Now;

                        _sqlRepository.LogPreAnalyze();

                        findCommand.State = CommandState.Analyzed;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }
                    catch (Exception e)
                    {
                        findCommand.State = CommandState.Fraction;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }

                }
            }


            return result;
        }

        private static async Task<bool> CalendarAddOrUpdateAnalyzer(List<Command> commands)
        {
            bool result = false;


            if (commands != null && commands.Count() > 0)
            {
                foreach (var cmd in commands)
                {
                    SQLRepository _sqlRepository = new SQLRepository(ConfigurationManager.ConnectionStrings["wskhContext"].ConnectionString);

                    var _commandService = DependencyResolver.Current.GetService<ICommandService>();

                    var findCommand = _commandService.FindById(cmd.Id);



                    try
                    {
                        findCommand.State = CommandState.Analyzing;

                        _commandService.Update(findCommand);

                        findCommand.StartingDateTime = DateTime.Now;


                        findCommand.State = CommandState.Analyzed;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }
                    catch (Exception e)
                    {
                        findCommand.State = CommandState.Fraction;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }

                }
            }


            return result;
        }

        private static async Task<bool> SpecialDayAnalyzer(List<Command> commands)
        {
            bool result = false;


            if (commands != null && commands.Count() > 0)
            {
                foreach (var cmd in commands)
                {
                    SQLRepository _sqlRepository = new SQLRepository(ConfigurationManager.ConnectionStrings["wskhContext"].ConnectionString);

                    var _commandService = DependencyResolver.Current.GetService<ICommandService>();

                    var findCommand = _commandService.FindById(cmd.Id);



                    try
                    {
                        findCommand.State = CommandState.Analyzing;

                        _commandService.Update(findCommand);

                        findCommand.StartingDateTime = DateTime.Now;


                        findCommand.State = CommandState.Analyzed;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }
                    catch (Exception e)
                    {
                        findCommand.State = CommandState.Fraction;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }

                }
            }


            return result;
        }

        private static async Task<bool> EnrollDeAssignAnalyzer(List<Command> commands)
        {
            bool result = false;


            if (commands != null && commands.Count() > 0)
            {
                foreach (var cmd in commands)
                {
                    SQLRepository _sqlRepository = new SQLRepository(ConfigurationManager.ConnectionStrings["wskhContext"].ConnectionString);

                    var _commandService = DependencyResolver.Current.GetService<ICommandService>();

                    var findCommand = _commandService.FindById(cmd.Id);



                    try
                    {
                        findCommand.State = CommandState.Analyzing;

                        _commandService.Update(findCommand);

                        findCommand.StartingDateTime = DateTime.Now;

                        //CalendarHistoryHelper.UpdateBySpecialDay(cmd.EntityId.GetValueOrDefault());

                        findCommand.State = CommandState.Analyzed;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }
                    catch (Exception e)
                    {
                        findCommand.State = CommandState.Fraction;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }

                }
            }


            return result;
        }

        private static async Task<bool> EnrollAssignAnalyzer(List<Command> commands)
        {
            bool result = false;


            if (commands != null && commands.Count() > 0)
            {
                foreach (var cmd in commands)
                {
                    SQLRepository _sqlRepository = new SQLRepository(ConfigurationManager.ConnectionStrings["wskhContext"].ConnectionString);

                    var _commandService = DependencyResolver.Current.GetService<ICommandService>();

                    var findCommand = _commandService.FindById(cmd.Id);



                    try
                    {
                        findCommand.State = CommandState.Analyzing;

                        _commandService.Update(findCommand);

                        findCommand.StartingDateTime = DateTime.Now;

                        findCommand.State = CommandState.Analyzed;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }
                    catch (Exception e)
                    {
                        findCommand.State = CommandState.Fraction;

                        findCommand.FinishDateTime = DateTime.Now;

                        _commandService.Update(findCommand);

                        _commandService.Dispose();
                    }

                }
            }


            return result;
        }
    }
}
