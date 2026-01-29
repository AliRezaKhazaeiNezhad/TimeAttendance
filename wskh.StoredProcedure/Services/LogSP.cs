using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Model;
using wskh.Data;

namespace wskh.StoredProcedure.Services
{
    public class LogSP
    {
        public LogSP()
        {

        }

        public List<FullLogModel> List()
        {
            wskhContext context = new wskhContext();
            var list = context.Database.SqlQuery<FullLogModel>("fullLogReport").ToList<FullLogModel>();
            return list;
        }


        public List<FullAnalayzedModel> AnalyzeList()
        {
            List<FullAnalayzedModel> modelList = new List<FullAnalayzedModel>();
            List<FullLogModel> entityList = List().OrderBy(x => x.LogDate.Date).ToList();

            var userList = entityList.Where(x => !string.IsNullOrEmpty(x.UserId)).ToList();
            var enrollList = entityList.Where(x => string.IsNullOrEmpty(x.UserId)).ToList();

            var userGroupList = userList.GroupBy(x => new { x.LogDate.Date, x.UserId });
            var enrollGroupList = userList.GroupBy(x => new { x.LogDate.Date, x.EnrollNo });

            foreach (var item in userGroupList)
            {
                FullAnalayzedModel model = new FullAnalayzedModel();
                var orderList = item.OrderBy(x => x.LogDate.Date).ToList();
                int index = 1;


                foreach (var item2 in orderList)
                {
                    item2.Index = index;

                    if (item2.Index % 2 == 0)
                        model.Logs = $"{model.Logs } <br/> <span class='exit'>{item2.LogTime}</span> </div>";
                    else
                        model.Logs = $"{model.Logs } <div class='startTime'> <span class='enterance'>{item2.LogTime}</span>";

                    model.FullLogModels.Add(item2);
                    index = 1 + index;
                }

                modelList.Add(model);
            }


            foreach (var item in enrollGroupList)
            {
                FullAnalayzedModel model = new FullAnalayzedModel();
                var orderList = item.OrderBy(x => x.LogDate.Date).ToList();
                int index = 1;


                foreach (var item2 in orderList)
                {
                    item2.Index = index;

                    if (item2.Index % 2 == 0)
                        model.Logs = $"{model.Logs } <br/> <span class='exit'>{item2.LogTime}</span> </div>";
                    else
                        model.Logs = $"{model.Logs } <div class='startTime'> <span class='enterance'>{item2.LogTime}</span>";

                    model.FullLogModels.Add(item2);
                    index = 1 + index;
                }

                modelList.Add(model);
            }


            return modelList;
        }
    }
}
