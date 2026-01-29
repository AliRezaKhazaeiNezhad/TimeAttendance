using System;
using System.Collections.Generic;
using System.Linq;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class EmploymentTypeService : IEmploymentTypeService
    {
        #region Ctor And Propertice 
        private IRepository<EmploymentType> _repository { get; set; }
        public EmploymentTypeService(IRepository<EmploymentType> repository)
        {
            _repository = repository;
        }
        public List<EmploymentType> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(EmploymentType entity)
        {
            _repository.Create(entity);
        }
        public void Update(EmploymentType entity)
        {
            _repository.Update(entity);
        }
        public void Delete(EmploymentType entity)
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

            List<EmploymentType> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

            return list.Count();
        }
        public EmploymentType FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<EmploymentType> List()
        {
            return GetList;
        }
        public List<EmploymentType> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
