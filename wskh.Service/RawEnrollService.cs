using System;
using System.Collections.Generic;
using System.Linq;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class RawEnrollService : IRawEnrollService
    {
        #region Ctor And Propertice 
        private IRepository<RawEnroll> _repository { get; set; }
        public RawEnrollService(IRepository<RawEnroll> repository)
        {
            _repository = repository;
        }
        public List<RawEnroll> GetList
        {
            get
            {
                return _repository.List();
            }
        }
        #endregion

        #region Methods
        public void Create(RawEnroll entity)
        {
            _repository.Create(entity);
        }
        public void Update(RawEnroll entity)
        {
            _repository.Update(entity);
        }
        public void Delete(RawEnroll entity)
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

            List<RawEnroll> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.ToList();

            return list.Count();
        }
        public RawEnroll FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<RawEnroll> List()
        {
            return GetList;
        }
        #endregion
    }
}
