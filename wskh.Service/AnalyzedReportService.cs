using System;
using System.Collections.Generic;
using System.Linq;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class AnalyzedReportService : IAnalyzedReportService
    {
        #region Ctor And Propertice 
        private IRepository<AnalyzedReport> _repository { get; set; }
        public AnalyzedReportService(IRepository<AnalyzedReport> repository)
        {
            _repository = repository;
        }
        public List<AnalyzedReport> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(AnalyzedReport entity)
        {
            _repository.Create(entity);
        }
        public void Update(AnalyzedReport entity)
        {
            _repository.Update(entity);
        }
        public void Delete(AnalyzedReport entity)
        {
            _repository.Delete(entity);
        }
        public int Count(string userId, DateTime stDate, DateTime edDate)
        {
            var list = GetList;

            if (list != null && list.Count() > 0)
                list = list.Where(x => x.UserId == userId && x.Date.Date >= stDate.Date && x.Date.Date <= edDate.Date).ToList();

            return list.Count();
        }
        public int Count(string search, string userId, DateTime stDate, DateTime edDate)
        {
            var list = GetList;

            if (list != null && list.Count() > 0)
                list = list.Where(x => x.UserId == userId && x.Date.Date >= stDate.Date && x.Date.Date <= edDate.Date).ToList();

            return list.Count();
        }
        public AnalyzedReport FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<AnalyzedReport> List(string userId, DateTime stDate, DateTime edDate)
        {
            var list = GetList;

            if (list != null && list.Count() > 0)
                list = list.Where(x => x.UserId == userId && x.Date.Date >= stDate.Date && x.Date.Date <= edDate.Date).ToList();

            return list;
        }
        public List<AnalyzedReport> FilterData(int start, int lenght, string search, string userId, DateTime stDate, DateTime edDate)
        {
            var list = GetList;

            if (list != null && list.Count() > 0)
                list = list.Where(x => x.UserId == userId && x.Date.Date >= stDate.Date && x.Date.Date <= edDate.Date).ToList();

            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
        #endregion
    }
}
