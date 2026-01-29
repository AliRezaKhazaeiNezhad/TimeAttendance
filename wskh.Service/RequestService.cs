using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Core.Enumerator;
using wskh.Data;

namespace wskh.Service
{
    public class RequestService : IRequestService
    {
        #region Ctor And Propertice 
        private IRepository<Request> _repository { get; set; }
        public RequestService(IRepository<Request> repository)
        {
            _repository = repository;
        }
        public List<Request> GetList
        {
            get
            {
                return _repository.List();
            }
        }
        #endregion

        #region Methods
        public void Create(Request entity)
        {
            _repository.Create(entity);
        }
        public void Update(Request entity)
        {
            _repository.Update(entity);
        }
        public void Delete(Request entity)
        {
            entity.Remove = true;
            _repository.Update(entity);
        }
        public int Count()
        {
            return GetList.Count();
        }
        public int Count(string search, RequestType type, RequestState state, string userId)
        {
            search = search.ToLower();

            List<Request> list = GetList;

            if (list != null)
            {

                if (!string.IsNullOrEmpty(userId))
                    list = list.Where(x => x.UserRequesterId == userId).ToList();

                list = list.Where(x => x.Type == type).ToList();
                if (state != RequestState.All)
                    list = list.Where(x => x.State == state).ToList();
                return list.Count();

            }
            else
            {
                return 0;
            }

            //if (!string.IsNullOrEmpty(search))
            //    list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

        }
        public Request FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<Request> List()
        {
            return GetList;
        }
        public List<Request> FilterData(int start, int lenght, string search, RequestType type, RequestState state, string userId)
        {
            var list = GetList;

            if (!string.IsNullOrEmpty(userId) && list != null)
                list = list.Where(x => x.UserRequesterId == userId).ToList();

            if (list != null)
                list = list.Where(x => x.Type == type).ToList();
            if (state != RequestState.All)
                list = list.Where(x => x.State == state).ToList();

            //if (!string.IsNullOrEmpty(search))
            //    list = list.Where(x => x.Title == search).ToList();
            int counter = 0;

            if (list != null)
            {
                return list.GetRange(start, Math.Min(lenght, counter - start));
            }
            else
            {
                return new List<Request>();
            }

        }
        #endregion
    }
}
