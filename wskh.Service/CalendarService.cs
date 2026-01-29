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
    public class CalendarService : ICalendarService
    {
        #region Ctor And Propertice 
        private IRepository<Calendar> _repository { get; set; }
        public CalendarService(IRepository<Calendar> repository)
        {
            _repository = repository;
        }
        public List<Calendar> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(Calendar entity)
        {
            _repository.Create(entity);
        }
        public void Update(Calendar entity)
        {
            _repository.Update(entity);
        }
        public void Delete(Calendar entity)
        {
            entity.Remove = true;
            _repository.Update(entity);
        }
        public int Count()
        {
            return GetList.Count();
        }
        public int Count(string search)
        {
            search = search.ToLower();

            List<Calendar> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

            return list.Count();
        }
        public Calendar FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<Calendar> List()
        {
            return GetList;
        }
        public List<Calendar> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
