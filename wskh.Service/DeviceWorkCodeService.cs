using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Data;

namespace TimeAttendance.Service
{
    public class DeviceWorkCodeService : IDeviceWorkCodeService
    {
        #region Ctor And Propertice 
        private IRepository<DeviceWorkCode> _repository { get; set; }
        public DeviceWorkCodeService(IRepository<DeviceWorkCode> repository)
        {
            _repository = repository;
        }
        public List<DeviceWorkCode> GetList
        {
            get
            {
                return _repository.List().OrderByDescending(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods

        public void Create(DeviceWorkCode entity)
        {
            _repository.Create(entity);
        }
        public void Update(DeviceWorkCode entity)
        {
            _repository.Update(entity);
        }
        public void Delete(DeviceWorkCode entity)
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

            List<DeviceWorkCode> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.ToList();

            return list.Count();
        }
        public DeviceWorkCode FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<DeviceWorkCode> List()
        {
            return GetList;
        }
        #endregion
    }
}
