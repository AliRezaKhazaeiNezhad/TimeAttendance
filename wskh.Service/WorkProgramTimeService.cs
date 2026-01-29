using System;
using System.Collections.Generic;
using System.Linq;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class WorkProgramTimeService : IWorkProgramTimeService
    {
        #region Ctor And Propertice 
        private IRepository<WorkProgramTime> _repository { get; set; }
        public WorkProgramTimeService(IRepository<WorkProgramTime> repository)
        {
            _repository = repository;
        }
        public List<WorkProgramTime> GetList
        {
            get
            {
                return _repository.List().OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(WorkProgramTime entity)
        {
            _repository.Create(entity);
        }
        public void Update(WorkProgramTime entity)
        {
            _repository.Update(entity);
        }
        public void Delete(WorkProgramTime entity)
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

            List<WorkProgramTime> list = GetList;
            //if (!string.IsNullOrEmpty(search))
            //    list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

            return list.Count();
        }
        public WorkProgramTime FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<WorkProgramTime> List()
        {
            return GetList;
        }
        public List<WorkProgramTime> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            //if (!string.IsNullOrEmpty(search))
            //    list = list.Where(x => x.Title == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
