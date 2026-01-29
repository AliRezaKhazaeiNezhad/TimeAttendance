using System;
using System.Collections.Generic;
using System.Linq;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class FingerDeviceService : IFingerDeviceService
    {
        #region Ctor And Propertice 
        private IRepository<FingerDevice> _repository { get; set; }
        public FingerDeviceService(IRepository<FingerDevice> repository)
        {
            _repository = repository;
        }
        public List<FingerDevice> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderByDescending(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public List<FingerDevice> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        public void Create(FingerDevice entity)
        {
            _repository.Create(entity);
        }
        public void Update(FingerDevice entity)
        {
            _repository.Update(entity);
        }
        public void Delete(FingerDevice entity)
        {
            _repository.Delete(entity);
        }
        public int Count()
        {
            return GetList.Count();
        }
        public int Count(string search)
        {
            search = search.ToLower();

            List<FingerDevice> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.ToList();

            return list.Count();
        }
        public FingerDevice FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<FingerDevice> List()
        {
            return GetList;
        }
        #endregion
    }
}
