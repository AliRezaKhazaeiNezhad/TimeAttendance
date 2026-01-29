using System;
using System.Collections.Generic;
using System.Linq;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class OrganizationBranchService : IOrganizationBranchService
    {
        #region Ctor And Propertice 
        private IRepository<OrganizationBranch> _repository { get; set; }
        public OrganizationBranchService(IRepository<OrganizationBranch> repository)
        {
            _repository = repository;
        }
        public List<OrganizationBranch> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(OrganizationBranch entity)
        {
            _repository.Create(entity);
        }
        public void Update(OrganizationBranch entity)
        {
            _repository.Update(entity);
        }
        public void Delete(OrganizationBranch entity)
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

            List<OrganizationBranch> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

            return list.Count();
        }
        public OrganizationBranch FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<OrganizationBranch> List()
        {
            return GetList;
        }
        public List<OrganizationBranch> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
