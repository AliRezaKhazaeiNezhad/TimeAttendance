using System;
using System.Collections.Generic;
using System.Linq;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class OrganizationLevelService : IOrganizationLevelService
    {
        #region Ctor And Propertice 
        private IRepository<OrganizationLevel> _repository { get; set; }
        public OrganizationLevelService(IRepository<OrganizationLevel> repository)
        {
            _repository = repository;
        }
        public List<OrganizationLevel> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(OrganizationLevel entity)
        {
            _repository.Create(entity);
        }
        public void Update(OrganizationLevel entity)
        {
            _repository.Update(entity);
        }
        public void Delete(OrganizationLevel entity)
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

            List<OrganizationLevel> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

            return list.Count();
        }
        public OrganizationLevel FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<OrganizationLevel> List()
        {
            return GetList;
        }
        public List<OrganizationLevel> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
