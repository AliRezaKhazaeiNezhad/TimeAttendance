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
    public class SpecialDayService : ISpecialDayService
    {
        #region Ctor And Propertice 
        private IRepository<SpecialDay> _repository { get; set; }
        public SpecialDayService(IRepository<SpecialDay> repository)
        {
            _repository = repository;
        }
        public List<SpecialDay> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(SpecialDay entity)
        {
            _repository.Create(entity);
        }
        public void Update(SpecialDay entity)
        {
            _repository.Update(entity);
        }
        public void Delete(SpecialDay entity)
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

            List<SpecialDay> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

            return list.Count();
        }
        public SpecialDay FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<SpecialDay> List()
        {
            return GetList;
        }
        public List<SpecialDay> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
        #endregion
    }
}
