using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Ctor And Propertices
        private readonly wskhContext _context;
        public Repository(wskhContext context)
        {
            _context = context;
        }

        public IDbSet<T> Context
        {
            get
            {
                return _context.Set<T>();
            }
        }
        #endregion


        #region Methods For Web
        public void Dispose()
        {
            _context.Dispose();
        }

        public void Create(T entity)
        {
            try
            {
                Context.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
            }
        }


        public void Update(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
            }
        }

        public void Delete(T entity)
        {
            try
            {
                Context.Remove(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
            }
        }



        public int Count()
        {
            try
            {
                var list = _context.Set<T>().ToList();
                return list.Count();
            }
            catch (Exception e)
            {
                return 0;
            }
        }


        public T FindById(int id)
        {
            try
            {
                var entity = _context.Set<T>().Find(id);
                return entity;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<T> List()
        {
            try
            {
                var list = _context.Set<T>().ToList();
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion
    }
}
