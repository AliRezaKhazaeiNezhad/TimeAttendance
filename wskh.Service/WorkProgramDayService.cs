using System;
using System.Collections.Generic;
using System.Linq;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Core.Enumerator;
using wskh.Data;

namespace wskh.Service
{
    public class WorkProgramDayService : IWorkProgramDayService
    {
        #region Ctor And Propertice 
        private IRepository<WorkProgramDay> _repository { get; set; }
        public WorkProgramDayService(IRepository<WorkProgramDay> repository)
        {
            _repository = repository;
        }
        public List<WorkProgramDay> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(WorkProgramDay entity)
        {
            _repository.Create(entity);
        }
        public void Update(WorkProgramDay entity)
        {
            _repository.Update(entity);
        }
        public void Delete(WorkProgramDay entity)
        {
            _repository.Delete(entity);
        }
        public int Count()
        {
            return GetList.Count();
        }
        public int Count(WorkType type)
        {

            List<WorkProgramDay> list = GetList;
            list = list.Where(x => x.WorkType == type).ToList();

            return list.Count();
        }
        public WorkProgramDay FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<WorkProgramDay> List(WorkType type)
        {
            var list = GetList;
            list = list.Where(x => x.WorkType == type).ToList();

            return list;
        }
        public List<WorkProgramDay> FilterData(int start, int lenght, WorkType type)
        {
            var list = GetList;
            list = list.Where(x => x.WorkType == type).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        public void Dispose()
        {
            _repository.Dispose();
        }
        #endregion
    }
}
