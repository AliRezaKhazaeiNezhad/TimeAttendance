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
    public class UserGroupService : IUserGroupService
    {
        #region Ctor And Propertice 
        private IRepository<UserGroup> _repository { get; set; }
        public UserGroupService(IRepository<UserGroup> repository)
        {
            _repository = repository;
        }
        public List<UserGroup> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(UserGroup entity)
        {
            _repository.Create(entity);
        }
        public void Update(UserGroup entity)
        {
            _repository.Update(entity);
        }
        public void Delete(UserGroup entity)
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

            List<UserGroup> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

            return list.Count();
        }
        public UserGroup FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<UserGroup> List()
        {
            return GetList;
        }
        public List<UserGroup> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
