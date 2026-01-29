using System;
using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface IReportDayService
    {
        List<ReportDay> GetList { get; }

        int AnalyzeCount(string search, string userId = null, DateTime? stDate = null, DateTime? edDate = null);
        List<ReportDay> AnalyzeFilterData(int start, int lenght, string search, string userId = null, DateTime? stDate = null, DateTime? edDate = null);
        int Count();
        int Count(string search);
        int Count(string search, DateTime? stDate = null, DateTime? edDate = null, int deviceId = 0, string userId = null, int enrollId = 0);
        void Create(ReportDay entity);
        void Delete(ReportDay entity);
        void Dispose();
        List<ReportDay> FilterData(int start, int lenght, string search);
        ReportDay FindById(int id);
        int InstantCount(string search, int type = 0);
        List<ReportDay> InstantFilterData(int start, int lenght, string search, int type = 0);
        List<ReportDay> List();
        int TradeCount(string search);
        int TradeCount(string search, int groupId = 0, string userId = "", DateTime? startDate = null, DateTime? endDate = null, int tradeTypeId = 0);
        List<ReportDay> TradeFilterData(int start, int lenght, string search);
        List<ReportDay> TradeFilterData(int start, int lenght, string search, int groupId = 0, string userId = "", DateTime? startDate = null, DateTime? endDate = null, int tradeTypeId = 0, int soirtCol = 0, string sortDir = "");
        void Update(ReportDay entity);
    }
}