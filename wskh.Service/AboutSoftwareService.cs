using System;
using System.Collections.Generic;
using System.Linq;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class AboutSoftwareService : IAboutSoftwareService
    {
        #region Ctor And Propertice 
        private IRepository<AboutSoftware> _repository { get; set; }
        public AboutSoftwareService(IRepository<AboutSoftware> repository)
        {
            _repository = repository;
        }
        public List<AboutSoftware> GetList
        {
            get
            {
                return _repository.List().OrderByDescending(x => x.PublishDateTime.Date).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(AboutSoftware entity)
        {
            _repository.Create(entity);
        }
        public void Update(AboutSoftware entity)
        {
            _repository.Update(entity);
        }
        public void Delete(AboutSoftware entity)
        {
            _repository.Delete(entity);
        }
        public int Count()
        {
            return GetList.Count();
        }
        public AboutSoftware FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<AboutSoftware> List()
        {
            return GetList;
        }
        #endregion
    }
}
