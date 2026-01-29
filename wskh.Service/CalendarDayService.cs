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
    public class CalendarDayService : ICalendarDayService
    {
        #region Ctor And Propertice 
        private IRepository<CalendarDay> _repository { get; set; }
        public CalendarDayService(IRepository<CalendarDay> repository)
        {
            _repository = repository;
        }
        public List<CalendarDay> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(CalendarDay entity)
        {
            _repository.Create(entity);
        }
        public void Update(CalendarDay entity)
        {
            _repository.Update(entity);
        }
        public void Delete(CalendarDay entity)
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
            List<CalendarDay> list = GetList;
            return list.Count();
        }
        public CalendarDay FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<CalendarDay> List()
        {
            return GetList;
        }
        public List<CalendarDay> FilterData(int start, int lenght, string search)
        {
            var list = GetList;
            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
