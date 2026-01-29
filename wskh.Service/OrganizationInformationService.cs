using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class OrganizationInformationService : IOrganizationInformationService
    {
        #region Ctor And Propertice 
        private IRepository<OrganizationInformation> _repository { get; set; }
        public OrganizationInformationService(IRepository<OrganizationInformation> repository)
        {
            _repository = repository;
        }
        public List<OrganizationInformation> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(OrganizationInformation entity)
        {
            _repository.Create(entity);
        }
        public void Update(OrganizationInformation entity)
        {
            _repository.Update(entity);
        }
        public void Delete(OrganizationInformation entity)
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

            List<OrganizationInformation> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

            return list.Count();
        }
        public OrganizationInformation FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<OrganizationInformation> List()
        {
            return GetList;
        }
        public List<OrganizationInformation> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
