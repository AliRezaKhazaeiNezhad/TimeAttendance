using System;
using System.Collections.Generic;
using System.Linq;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class EnrollService : IEnrollService
    {
        #region Ctor And Propertice 
        private IRepository<Enroll> _repository { get; set; }
        public EnrollService(IRepository<Enroll> repository)
        {
            _repository = repository;
        }
        public List<Enroll> GetList
        {
            get
            {
                return _repository.List();
            }
        }
        #endregion

        #region Methods

        public void Dispose()
        {
            _repository.Dispose();
        }
        public void Create(Enroll entity)
        {
            _repository.Create(entity);
        }
        public void Update(Enroll entity)
        {
            _repository.Update(entity);
        }
        public void Delete(Enroll entity)
        {
            _repository.Delete(entity);
        }
        public int Count(int deviceId)
        {
            var list = GetList;
            list = list.Where(x => x.FingerDeviceId == deviceId).ToList();
            return list.Count();
        }
        public int Count(string search, int deviceId)
        {
            List<Enroll> list = GetList;
            return list.Count();
        }
        public Enroll FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<Enroll> List()
        {
            return GetList;
        }
        public List<Enroll> FilterData(int start, int lenght, string search, int deviceId)
        {
            var list = GetList;
            list = list.Where(x => x.FingerDeviceId == deviceId).ToList();
            if (list.Count < start)
                start = 0;

            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
