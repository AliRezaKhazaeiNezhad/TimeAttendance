using System;
using System.Collections.Generic;
using System.Linq;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public partial class LogService : ILogService
    {
        #region Ctor And Propertice 
        private IRepository<Log> _repository { get; set; }
        public LogService(IRepository<Log> repository)
        {
            _repository = repository;
        }
        public List<Log> GetList
        {
            get
            {
                return _repository.List().OrderByDescending(x => x.LogDate.GetValueOrDefault().Date).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(Log entity)
        {
            _repository.Create(entity);
        }
        public void Update(Log entity)
        {
            _repository.Update(entity);
        }
        public void Delete(Log entity)
        {
            _repository.Delete(entity);
        }


        public List<Log> FilterDataRemovedLogs(int start, int lenght)
        {
            var list = _repository.List();

            if (list != null && list.Count() > 0)
                list = list.Where(x => x.Remove == true).OrderByDescending(x => x.Id).ToList();

            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        public int CountRemovedLogs()
        {
            var list = _repository.List();

            if (list != null && list.Count() > 0)
                list = list.Where(x => x.Remove == true).OrderByDescending(x => x.Id).ToList();

            return list.Count();
        }



        public int Count(int deviceId)
        {
            var list = GetList;
            list = list.Where(x => x.DeviceId != null && x.DeviceId == deviceId).ToList();
            return list.Count();
        }
        public int Count(string search, int deviceId)
        {
            List<Log> list = GetList;
            list = list.Where(x => x.DeviceId != null && x.DeviceId == deviceId).ToList();

            return list.Count();
        }
        public int Count(string search)
        {
            List<Log> list = GetList;

            return list.Count();
        }
        public Log FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<Log> List()
        {
            return GetList;
        }
        public List<Log> FilterData(int start, int lenght, string search, int deviceId)
        {
            var list = GetList;
            list = list.Where(x => x.DeviceId == deviceId).ToList();
            if (start > list.Count)
                start = 1;

            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        public List<Log> FilterData(int start, int lenght, string search)
        {
            var list = GetList;
            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }



        public List<LogModelFilterData> FilterData2(int start = 0, int lenght = 0, string search = null, DateTime? startDate = null, DateTime? endDate = null, int deviceId = 0, int enrollId = 0, string userId = null)
        {
            var list = GetList;

            list = list.OrderByDescending(x => x.LogDate.GetValueOrDefault().Date).ThenBy(x => x.LogDate.GetValueOrDefault().Date).ToList();

            if (startDate != null)
                list = list.Where(x => x.LogDate.GetValueOrDefault().Date >= startDate.GetValueOrDefault().Date).ToList();

            if (endDate != null)
                list = list.Where(x => x.LogDate.GetValueOrDefault().Date <= endDate.GetValueOrDefault().Date).ToList();

            if (deviceId > 0)
                list = list.Where(x => x.DeviceId == deviceId).ToList();


            if (enrollId > 0)
                list = list.Where(x => x.EnrollId == enrollId).ToList();


            if (userId != null)
                list = list.Where(x => x.Enroll != null && x.Enroll.UserId != null && x.Enroll.UserId == userId).ToList();

            List<LogModelFilterData> modelList = new List<LogModelFilterData>();
            DateTime defaultDate = new DateTime(1990, 01, 01, 00, 00, 00, 00);

            foreach (var item in list)
            {
                if (item.LogDate.GetValueOrDefault().Date > defaultDate.Date)
                {

                    if (modelList.Where(x => x.Logdate.Date == item.LogDate.GetValueOrDefault().Date).Count() == 0)
                    {
                        modelList.Add(new LogModelFilterData()
                        {
                            DeviceName = item.Device.Title,
                            Logdate = item.LogDate.GetValueOrDefault(),
                            EnrollNo = item.EnrollNo.GetValueOrDefault(),
                            UserId = item.Enroll.UserId != null ? item.Enroll.UserId : null,
                            FirstName = item.Enroll.UserId != null ? item.Enroll.User.FirstName : null,
                            LastName = item.Enroll.UserId != null ? item.Enroll.User.Lastname : null,
                            Index = 1,
                            Logs = $"<div class='startTime'> <span class='enterance'>{item.LogTime}</span>"
                        });
                    }
                    else
                    {
                        var findItem = modelList.Where(x => x.Logdate.Date == item.LogDate.GetValueOrDefault().Date && ((x.EnrollNo == (item.Enroll == null ? -1000 : item.Enroll.EnrollNo)) || x.UserId == userId)).FirstOrDefault();

                        if (findItem != null)
                        {
                            findItem.Index = findItem.Index + 1;

                            if (findItem.Index % 2 == 0)
                                findItem.Logs = $"{findItem.Logs } <br/> <span class='exit'>{item.LogTime}</span> </div>";
                            else
                                findItem.Logs = $"{findItem.Logs } <div class='startTime'> <span class='enterance'>{item.LogTime}</span>";

                        }
                    }
                }
            }



            return modelList.GetRange(start, Math.Min(lenght, modelList.Count - start));
        }


        public int Count2(DateTime? startDate = null, DateTime? endDate = null, int deviceId = 0, int enrollId = 0, string userId = null)
        {
            var list = GetList;

            if (startDate != null)
                list = list.Where(x => x.LogDate.GetValueOrDefault().Date >= startDate.GetValueOrDefault().Date).ToList();

            if (endDate != null)
                list = list.Where(x => x.LogDate.GetValueOrDefault().Date <= endDate.GetValueOrDefault().Date).ToList();

            if (deviceId > 0)
                list = list.Where(x => x.DeviceId == deviceId).ToList();


            if (enrollId > 0)
                list = list.Where(x => x.EnrollId == enrollId).ToList();

            if (userId != null)
                list = list.Where(x => x.Enroll != null && x.Enroll.UserId != null && x.Enroll.UserId == userId).ToList();



            var groupList = list.GroupBy(x => x.LogDate.GetValueOrDefault().Date).ToList();
            List<LogModelFilterData> modelList = new List<LogModelFilterData>();
            DateTime defaultDate = new DateTime(1990, 01, 01, 00, 00, 00, 00);

            foreach (var item in groupList)
            {
                if (item.FirstOrDefault().LogDate.GetValueOrDefault().Date > defaultDate.Date)
                {
                    string logs = "";
                    foreach (var item2 in item)
                    {
                        logs = logs + " " + item2.LogTime;
                    }

                    modelList.Add(new LogModelFilterData()
                    {
                        DeviceName = item.FirstOrDefault().Device.Title,
                        Logdate = item.FirstOrDefault().LogDate.GetValueOrDefault(),
                        EnrollNo = item.FirstOrDefault().EnrollNo.GetValueOrDefault(),
                        UserId = item.FirstOrDefault().Enroll.UserId != null ? item.FirstOrDefault().Enroll.UserId : null,
                        FirstName = item.FirstOrDefault().Enroll.UserId != null ? item.FirstOrDefault().Enroll.User.FirstName : null,
                        LastName = item.FirstOrDefault().Enroll.UserId != null ? item.FirstOrDefault().Enroll.User.Lastname : null,
                        Logs = logs
                    });
                }

            }


            return modelList.Count();
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
        #endregion
    }

    public class LogModelFilterData
    {
        public LogModelFilterData()
        {

        }

        public int Index { get; set; }
        public string DeviceName { get; set; }
        public DateTime Logdate { get; set; }
        public int EnrollNo { get; set; }
        public string UserId { get; set; }
        public string Logs { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
