using System;
using System.Collections.Generic;
using TimeAttendance.Core;

namespace wskh.Service
{
    public interface IAnalyzedReportService
    {
        List<AnalyzedReport> GetList { get; }

        int Count(string userId, DateTime stDate, DateTime edDate);
        int Count(string search, string userId, DateTime stDate, DateTime edDate);
        void Create(AnalyzedReport entity);
        void Delete(AnalyzedReport entity);
        List<AnalyzedReport> FilterData(int start, int lenght, string search, string userId, DateTime stDate, DateTime edDate);
        AnalyzedReport FindById(int id);
        List<AnalyzedReport> List(string userId, DateTime stDate, DateTime edDate);
        void Update(AnalyzedReport entity);

        void Dispose();
    }
}