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
using wskh.FingerTec;
using wskh.LogAndEnrlol.analyzer.SQL;
using wskh.Service;

namespace wskh.Web.Helper.Jobs
{
    [DisallowConcurrentExecution]
    public class LogAndEnrollJob : IJob
    {
        public LogAndEnrollJob()
        {

        }
        public Task Execute(IJobExecutionContext context)
        {

            string ConnectorName = ConfigurationManager.AppSettings["ConnectorName"];
            string ConnectorCode = ConfigurationManager.AppSettings["ConnectorCode"];


            var _deviceService = DependencyResolver.Current.GetService<IFingerDeviceService>();
            var _rawEnrollService = DependencyResolver.Current.GetService<IRawEnrollService>();
            var _patchHistoryService = DependencyResolver.Current.GetService<IPatchHistoryService>();


            SQLRepository repo = new SQLRepository(ConnectionHelper.Get());


            var deviceList = _deviceService.GetList;
            foreach (var device in deviceList)
            {
                int lastLogCount = 0;
                var findLastPatchHistory = _patchHistoryService.GetList.OrderByDescending(x => x.CreateDateTime.Date).ThenByDescending(x => x.CreateDateTime.TimeOfDay).FirstOrDefault(x => x.FingerDeviceId == device.Id);

                if (findLastPatchHistory == null || findLastPatchHistory.LastLogCount <= 0)
                    lastLogCount = 0;
                else
                    lastLogCount = findLastPatchHistory.LastLogCount;

                FingerTec.FingerTec fingerTecConnector = FingerTecHelper.Connector(ConnectionType.TCP, device.Id);
                fingerTecConnector.Connect();
                if (lastLogCount > 0)
                    fingerTecConnector.SetLastLog(lastLogCount);

                BulkCRUD.InsertLog(fingerTecConnector, device);

                var list = fingerTecConnector.GetEnrolls();
                list.ForEach(x =>
                     _rawEnrollService.Create(new RawEnroll()
                     {
                         Enabled = x.Enable,
                         EnrollNo = x.EnrollNo,
                         Name = x.Name,
                         Password = x.PassWord,
                         Privileg = x.Privilage,
                         DeviceId = device.Id
                     })
                    );
                fingerTecConnector.DisConnect();
                repo.FullAnanlyze();

                _patchHistoryService.Create(new PatchHistory()
                {
                    CreateDateTime = DateTime.Now,
                    Description = "درج ورود و خروج و کاربر سخت افزار",
                    FingerDeviceId = device.Id,
                    PtachName = ConnectorName,
                    PatchCode = ConnectorCode,
                    LastLogCount = (lastLogCount + fingerTecConnector.LogCount),
                    LogCount = fingerTecConnector.LogCount
                });
            }

            return Task.CompletedTask;
        }
    }
}