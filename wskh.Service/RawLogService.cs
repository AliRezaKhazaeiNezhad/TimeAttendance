using System;
using System.Collections.Generic;
using System.Linq;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class RawLogService : IRawLogService
    {
        #region Ctor And Propertice 
        private IRepository<RawLog> _repository { get; set; }
        public RawLogService(IRepository<RawLog> repository)
        {
            _repository = repository;
        }
        public List<RawLog> GetList
        {
            get
            {
                return _repository.List();
            }
        }
        #endregion

        #region Methods
        public void Create(RawLog entity)
        {
            _repository.Create(entity);
        }
        public void Update(RawLog entity)
        {
            _repository.Update(entity);
        }
        public void Delete(RawLog entity)
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

            List<RawLog> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.ToList();

            return list.Count();
        }
        public RawLog FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<RawLog> List()
        {
            return GetList;
        }

        public List<RawLog> FilterData(int start, int lenght, string search)
        {
            var list = GetList;
            list = list.OrderByDescending(x => x.LogDate).ThenByDescending(x => x.LogTime).ToList();
            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
