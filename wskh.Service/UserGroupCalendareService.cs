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
    public class UserGroupCalendareService : IUserGroupCalendareService
    {
        #region Ctor And Propertice 
        private IRepository<UserGroupCalendare> _repository { get; set; }
        public UserGroupCalendareService(IRepository<UserGroupCalendare> repository)
        {
            _repository = repository;
        }
        public List<UserGroupCalendare> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(UserGroupCalendare entity)
        {
            _repository.Create(entity);
        }
        public void Update(UserGroupCalendare entity)
        {
            _repository.Update(entity);
        }
        public void Delete(UserGroupCalendare entity)
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

            List<UserGroupCalendare> list = GetList;
            //if (!string.IsNullOrEmpty(search))
            //    list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

            return list.Count();
        }
        public UserGroupCalendare FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<UserGroupCalendare> List()
        {
            return GetList;
        }
        public List<UserGroupCalendare> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            //if (!string.IsNullOrEmpty(search))
            //    list = list.Where(x => x.Title == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
