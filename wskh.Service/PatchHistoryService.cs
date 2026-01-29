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
    public class PatchHistoryService : IPatchHistoryService
    {
        #region Ctor And Propertice 
        private IRepository<PatchHistory> _repository { get; set; }
        public PatchHistoryService(IRepository<PatchHistory> repository)
        {
            _repository = repository;
        }
        public List<PatchHistory> GetList
        {
            get
            {
                return _repository.List().OrderByDescending(x => x.CreateDateTime).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(PatchHistory entity)
        {
            _repository.Create(entity);
        }
        public void Update(PatchHistory entity)
        {
            _repository.Update(entity);
        }
        public void Delete(PatchHistory entity)
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

            List<PatchHistory> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Description == search || x.PtachName == search || x.FingerDevice.Title == search || x.PatchCode == search).ToList();

            return list.Count();
        }
        public PatchHistory FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<PatchHistory> List()
        {
            return GetList;
        }
        public List<PatchHistory> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Description == search || x.PtachName == search || x.FingerDevice.Title == search || x.PatchCode == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
