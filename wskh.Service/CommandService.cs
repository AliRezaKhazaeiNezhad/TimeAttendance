using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Core.Enumerator;
using wskh.Data;

namespace wskh.Service
{
    public class CommandService : ICommandService
    {
        #region Ctor And Propertice 
        private IRepository<Command> _repository { get; set; }
        public CommandService(IRepository<Command> repository)
        {
            _repository = repository;
        }
        public List<Command> GetList
        {
            get
            {
                return _repository.List().Where(x => x.Remove == false).OrderByDescending(x => x.CreateDateTime.GetValueOrDefault().Date).ThenByDescending(x => x.CreateDateTime.GetValueOrDefault().TimeOfDay).ToList();
            }
        }
        #endregion

        #region Methods
        public void Create(Command entity)
        {
            _repository.Create(entity);
        }
        public void Update(Command entity)
        {
            _repository.Update(entity);
        }
        public void Delete(Command entity)
        {
            _repository.Delete(entity);
        }
        public int Count(CommandCategory cat)
        {
            var list = GetList;
            //list = list.Where(x => x.CommandCategory == cat).ToList();
            return GetList.Count();
        }
        public Command FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<Command> List()
        {
            return GetList;
        }
        public void Dispose()
        {
            _repository.Dispose();
        }
        public List<Command> FilterData(int start, int lenght, CommandCategory cat)
        {
            var list = GetList;
            //list = list.Where(x => x.CommandCategory == cat).ToList();
            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }
        #endregion
    }
}
