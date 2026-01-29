using System;
using System.Collections.Generic;
using System.Linq;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class ReportDayService : IReportDayService
    {
        #region Ctor And Propertice 
        private IRepository<ReportDay> _repository { get; set; }
        public ReportDayService(IRepository<ReportDay> repository)
        {
            _repository = repository;
        }
        public List<ReportDay> GetList
        {
            get
            {
                return _repository.List().ToList();
            }
        }
        #endregion

        #region Methods

        public void Dispose()
        {
            _repository.Dispose();
        }
        public void Create(ReportDay entity)
        {
            _repository.Create(entity);
        }
        public void Update(ReportDay entity)
        {
            _repository.Update(entity);
        }
        public void Delete(ReportDay entity)
        {
            _repository.Delete(entity);
        }
        public int Count()
        {
            var list = GetList;
            return list.Count();
        }
        public int Count(string search)
        {
            List<ReportDay> list = GetList;
            //list = list.Where(x => x.DeviceId != null && x.DeviceId == deviceId).ToList();
            return list.Count();
        }

        public ReportDay FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<ReportDay> List()
        {
            return GetList;
        }
        public List<ReportDay> FilterData(int start, int lenght, string search)
        {
            var list = GetList;
            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }


        public int Count(string search, DateTime? stDate = null, DateTime? edDate = null, int deviceId = 0, string userId = null, int enrollId = 0)
        {
            var list = GetList;


            if (stDate != null)
                list = list.Where(x => x.ReportDate >= stDate).ToList();

            if (edDate != null)
                list = list.Where(x => x.ReportDate <= edDate).ToList();



            return list.Count();
        }

        // type = 0  ---> برای کل گزارش های امروز
        // type = 1  ---> برای کل گزارش های حاضرین امروز
        // type = 2  ---> برای کل گزارش های غایبین امروز
        public List<ReportDay> InstantFilterData(int start, int lenght, string search, int type = 0)
        {
            var list = GetList;
            if (list != null && list.Count() > 0)
            {
                list = list.Where(x => x.ReportDate.Date == DateTime.Now.Date).ToList();
            }
            else
                list = new List<ReportDay>();

            switch (type)
            {
                case 0:
                default:
                    break;
                case 1:
                    if (list != null && list.Count() > 0)
                        list = list.Where(x => x.Logs.Count() > 0).ToList();
                    break;
                case 2:
                    if (list != null && list.Count() > 0)
                        list = list.Where(x => x.Logs.Count() <= 0).ToList();
                    break;
            }

            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }


        // type = 0  ---> برای کل گزارش های امروز
        // type = 1  ---> برای کل گزارش های حاضرین امروز
        // type = 2  ---> برای کل گزارش های غایبین امروز
        public int InstantCount(string search, int type = 0)
        {
            var list = GetList;
            if (list != null && list.Count() > 0)
            {
                list = list.Where(x => x.ReportDate.Date == DateTime.Now.Date).ToList();
            }
            else
                list = new List<ReportDay>();

            switch (type)
            {
                case 0:
                default:
                    break;
                case 1:
                    if (list != null && list.Count() > 0)
                        list = list.Where(x => x.Logs.Count() > 0).ToList();
                    break;
                case 2:
                    if (list != null && list.Count() > 0)
                        list = list.Where(x => x.Logs.Count() <= 0).ToList();
                    break;
            }

            return list.Count();
        }


        public List<ReportDay> TradeFilterData(int start, int lenght, string search)
        {
            var list = GetList;



            list = list
                .OrderByDescending(x => x.ReportDate.Date)
                .ThenByDescending(x => x.ReportDate.TimeOfDay)
                .ThenByDescending(x => x.UserId)
                .ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }


        public int TradeCount(string search)
        {
            var list = GetList;

            return list.Count();
        }




        public List<ReportDay> AnalyzeFilterData(int start, int lenght, string search, string userId = null, DateTime? stDate = null, DateTime? edDate = null)
        {
            var list = GetList;

            if (!string.IsNullOrEmpty(userId))
            {
                list = list.Where(x => x.UserId == userId).ToList();
            }

            if (stDate != null)
            {
                list = list.Where(x => x.ReportDate.Date >= stDate).ToList();
            }

            if (edDate != null)
            {
                list = list.Where(x => x.ReportDate.Date <= edDate).ToList();
            }

            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }


        public int AnalyzeCount(string search, string userId = null, DateTime? stDate = null, DateTime? edDate = null)
        {
            var list = GetList;


            if (!string.IsNullOrEmpty(userId))
            {
                list = list.Where(x => x.UserId == userId).ToList();
            }

            if (stDate != null)
            {
                list = list.Where(x => x.ReportDate.Date >= stDate).ToList();
            }

            if (edDate != null)
            {
                list = list.Where(x => x.ReportDate.Date <= edDate).ToList();
            }

            return list.Count();
        }






        public List<ReportDay> TradeFilterData(int start, int lenght, string search, int groupId = 0, string userId = "", DateTime? startDate = null, DateTime? endDate = null, int tradeTypeId = 0, int sortCol = 0, string sortDir = "")
        {
            var list = GetList;
            if (userId == "0")
                userId = null;

            if (list != null && list.Count > 0)
            {
                list = list.Where(x => x.ReportDate >= startDate && x.ReportDate <= endDate).ToList();

                if (!string.IsNullOrEmpty(userId))
                    list = list.Where(x => x.UserId == userId).ToList();

                if (groupId > 0)
                    list = list.Where(x => x.User != null && x.User.UserGroupId == groupId).ToList();

                switch (sortCol)
                {
                    case 1:
                        list = sortDir == "asc" ? list.OrderBy(x => x.User.UserGroup.Title).ToList() : list.OrderByDescending(x => x.User.UserGroup.Title).ToList();
                        break;
                    case 2:
                        list = sortDir == "asc" ? list.OrderBy(x => x.User.Lastname).ToList() : list.OrderByDescending(x => x.User.Lastname).ToList();
                        break;
                    case 4:
                        list = sortDir == "asc" ? list.OrderBy(x => x.ReportDate.Date).ToList() : list.OrderByDescending(x => x.ReportDate.Date).ToList();
                        break;
                    default:
                        break;
                }


                switch (tradeTypeId)
                {
                    case 0:
                    default:
                        break;

                    case 1:
                        list = list.Where(x => x.Logs != null && x.Logs.Count() > 0 && x.Logs.Where(f => f.Remove == false).Count() > 0 && x.Logs.Where(f => f.Remove == false).Count() % 2 == 0).ToList();
                        break;
                    case 2:
                        list = list.Where(x => x.Logs != null && x.Logs.Count() > 0 && x.Logs.Where(f => f.Remove == false).Count() > 0 && x.Logs.Where(f => f.Remove == false).Count() % 2 != 0).ToList();
                        break;
                }
            }

            if (list != null && list.Count() > 0)
                list = list.OrderBy(x => x.ReportDate.Date).ToList();
            if (start > list.Count)
            {
                start = 0;
            }

            if (list.Count() >= 10)
                return list.GetRange(start, Math.Min(lenght, list.Count - start));
            else
                return list;
        }


        public int TradeCount(string search, int groupId = 0, string userId = "", DateTime? startDate = null, DateTime? endDate = null, int tradeTypeId = 0)
        {
            var list = GetList;
            if (userId == "0")
                userId = null;

            if (list != null && list.Count > 0)
            {
                list = list.Where(x => x.ReportDate >= startDate && x.ReportDate <= endDate).ToList();

                if (!string.IsNullOrEmpty(userId))
                    list = list.Where(x => x.UserId == userId).ToList();

                if (groupId > 0)
                    list = list.Where(x => x.User != null && x.User.UserGroupId == groupId).ToList();

                switch (tradeTypeId)
                {
                    case 0:
                    default:
                        break;

                    case 1:
                        list = list.Where(x => x.Logs != null && x.Logs.Count() > 0 && x.Logs.Where(f => f.Remove == false).Count() > 0 && x.Logs.Where(f => f.Remove == false).Count() % 2 == 0).ToList();
                        break;
                    case 2:
                        list = list.Where(x => x.Logs != null && x.Logs.Count() > 0 && x.Logs.Where(f => f.Remove == false).Count() > 0 && x.Logs.Where(f => f.Remove == false).Count() % 2 != 0).ToList();
                        break;
                }
            }

            return list.Count();
        }
        #endregion
    }
}
