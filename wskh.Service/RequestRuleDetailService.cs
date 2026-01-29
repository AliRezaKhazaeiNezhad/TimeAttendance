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
    public class RequestRuleDetailService : IRequestRuleDetailService
    {
        #region Ctor And Propertice 
        private IRepository<RequestRuleDetail> _repository { get; set; }
        public RequestRuleDetailService(IRepository<RequestRuleDetail> repository)
        {
            _repository = repository;
        }
        public List<RequestRuleDetail> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(RequestRuleDetail entity)
        {
            _repository.Create(entity);
        }
        public void Update(RequestRuleDetail entity)
        {
            _repository.Update(entity);
        }
        public void Delete(RequestRuleDetail entity)
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

            List<RequestRuleDetail> list = GetList;
            //if (!string.IsNullOrEmpty(search))
            //    list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

            return list.Count();
        }
        public RequestRuleDetail FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<RequestRuleDetail> List()
        {
            return GetList;
        }
        public List<RequestRuleDetail> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            //if (!string.IsNullOrEmpty(search))
            //    list = list.Where(x => x.Title == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
