using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;
using wskh.Data;

namespace wskh.Service
{
    public class EducationLevelService : IEducationLevelService
    {
        #region Ctor And Propertice 
        private IRepository<EducationLevel> _repository { get; set; }
        public EducationLevelService(IRepository<EducationLevel> repository)
        {
            _repository = repository;
        }
        public List<EducationLevel> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderBy(x => x.Id).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(EducationLevel entity)
        {
            _repository.Create(entity);
        }
        public void Update(EducationLevel entity)
        {
            _repository.Update(entity);
        }
        public void Delete(EducationLevel entity)
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

            List<EducationLevel> list = GetList;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title.ToLower().Contains(search)).ToList();

            return list.Count();
        }
        public EducationLevel FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<EducationLevel> List()
        {
            return GetList;
        }
        public List<EducationLevel> FilterData(int start, int lenght, string search)
        {
            var list = GetList;

            if (!string.IsNullOrEmpty(search))
                list = list.Where(x => x.Title == search).ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
