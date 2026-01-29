using System;
using System.Collections.Generic;
using System.Linq;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Core.Enumerator;
using wskh.Data;

namespace wskh.Service
{
    public class WorkProgramService : IWorkProgramService
    {
        #region Ctor And Propertice 
        private IRepository<WorkProgram> _repository { get; set; }
        public WorkProgramService(IRepository<WorkProgram> repository)
        {
            _repository = repository;
        }
        public List<WorkProgram> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(WorkProgram entity)
        {
            _repository.Create(entity);
        }
        public void Update(WorkProgram entity)
        {
            _repository.Update(entity);
        }
        public void Delete(WorkProgram entity)
        {
            _repository.Delete(entity);
        }
        public int Count()
        {
            return GetList.Count();
        }
        public int Count(string search, WorkProgramType type)
        {
            search = search.ToLower();

            List<WorkProgram> list = GetList;
            if (type != WorkProgramType.All)
                list = list.Where(x => x.Type == WorkProgramType.Ordinary).ToList();

            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

            return list.Count();
        }
        public WorkProgram FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<WorkProgram> List(WorkProgramType type)
        {
            var list = GetList;
            if (type != WorkProgramType.All)
                list = list.Where(x => x.Type == type).ToList();

         

            return list;
        }
        public List<WorkProgram> FilterData(int start, int lenght, string search, WorkProgramType type)
        {
            var list = GetList;

            if (type != WorkProgramType.All)
                list = list.Where(x => x.Type == type).ToList();

            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
