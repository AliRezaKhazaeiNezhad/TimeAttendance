using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Data;

namespace TimeAttendance.Service
{
    public class DeviceCardService : IDeviceCardService
    {
        #region Ctor And Propertice 
        private IRepository<DeviceCard> _repository { get; set; }
        public DeviceCardService(IRepository<DeviceCard> repository)
        {
            _repository = repository;
        }
        public List<DeviceCard> GetList
        {
            get
            {
                return _repository.List().OrderByDescending(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods

        public void Create(DeviceCard entity)
        {
            _repository.Create(entity);
        }
        public void Update(DeviceCard entity)
        {
            _repository.Update(entity);
        }
        public void Delete(DeviceCard entity)
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

            List<DeviceCard> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.ToList();

            return list.Count();
        }
        public DeviceCard FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<DeviceCard> List()
        {
            return GetList;
        }
        #endregion
    }
}
