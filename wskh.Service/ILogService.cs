using System;
using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface ILogService
    {
        List<Log> GetList { get; }

        int Count(int deviceId);
        int Count(string search);
        int Count(string search, int deviceId);
        int Count2(DateTime? startDate = null, DateTime? endDate = null, int deviceId = 0, int enrollId = 0, string userId = null);
        void Create(Log entity);
        void Delete(Log entity);
        void Dispose();
        List<Log> FilterData(int start, int lenght, string search);
        List<Log> FilterData(int start, int lenght, string search, int deviceId);
        List<LogModelFilterData> FilterData2(int start = 0, int lenght = 0, string search = null, DateTime? startDate = null, DateTime? endDate = null, int deviceId = 0, int enrollId = 0, string userId = null);
        Log FindById(int id);
        List<Log> List();
        void Update(Log entity);
        List<Log> FilterDataRemovedLogs(int start, int lenght);
        int CountRemovedLogs();
    }
}