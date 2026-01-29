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
    public class UserService : IUserService
    {
        #region Ctor And Propertice 
        private IRepository<wskhUser> _repository { get; set; }
        public UserService(IRepository<wskhUser> repository)
        {
            _repository = repository;
        }
        public List<wskhUser> GetList
        {
            get
            {
                var adminRole = HashHelper.Encrypt("admin");
                return _repository.List().Where(x => x.UserRoleType != adminRole).ToList();
            }
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            _repository.Dispose();
        }

        public void Create(wskhUser entity)
        {
            _repository.Create(entity);
        }
        public void Update(wskhUser entity)
        {
            _repository.Update(entity);
        }
        public void Delete(wskhUser entity)
        {
            _repository.Delete(entity);
        }
        public int Count()
        {
            return GetList.Count();
        }
        public int Count(string search)
        {
            List<wskhUser> list = GetList;

            if (!string.IsNullOrEmpty(search))
                list = list
                    .Where(x =>
                    x.FirstName.ToLower().Contains(search) ||
                    x.Lastname.ToLower().Contains(search) ||
                    HashHelper.Decrypt(x.NationalCode).Contains(search))
                    .ToList();



            return list.Count();
        }
        public List<wskhUser> List()
        {
            return GetList;
        }
        public List<wskhUser> List2()
        {
            return _repository.List();
        }
        public List<wskhUser> FilterData(int start, int lenght, string search)
        {
            var list = GetList;


            if (!string.IsNullOrEmpty(search))
                list = list
                    .Where(x =>
                    x.FirstName.ToLower().Contains(search) ||
                    x.Lastname.ToLower().Contains(search) ||
                    HashHelper.Decrypt(x.NationalCode).Contains(search))
                    .ToList();


            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }




        // type = 0  ---> برای کل گزارش های امروز
        // type = 1  ---> برای کل گزارش های حاضرین امروز
        // type = 2  ---> برای کل گزارش های غایبین امروز
        public List<wskhUser> InstantFilterData(int start, int lenght, string search, int type = 0, string userId = null, int usergroupId = 0)
        {
            var currentDateTime = DateTime.Now;
            var list = GetList;
            if (list != null && list.Count() > 0)
            {
                list = list.OrderBy(x => x.ReportDays != null).ToList();
            }
            else
                list = new List<wskhUser>();

            if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(userId))
                list = list.Where(x => x.Id.Contains(userId)).ToList();

            if (list != null && list.Count() > 0 && usergroupId > 0)
                list = list.Where(x => x.UserGroupId == usergroupId).ToList();

            if (list != null && list.Count() > 0)
            {
                foreach (var user in list)
                {
                    if (user.ReportDays != null && user.ReportDays.Count() > 0)
                    {
                        user.ReportDays = user.ReportDays.Where(x => x.ReportDate.Date == currentDateTime.Date).ToList();
                        if (user.ReportDays != null && user.ReportDays.Count() > 0)
                        {
                            user.ReportDays.ForEach(x => x.Logs.OrderBy(f => f.LogTime).ToList());
                            foreach (var item in user.ReportDays)
                            {
                                item.Logs = item.Logs.Where(x => x.Remove == false).ToList();
                            }

                        }
                    }
                    else
                    {
                        user.ReportDays = new List<ReportDay>();
                    }
                }

            }



            switch (type)
            {
                case 0:
                default:
                    if (list != null && list.Count() > 0)
                    {
                        list = list.OrderByDescending(x =>
                        x.ReportDays != null &&
                        x.ReportDays.Count() >= 0)
                            .ToList();
                    }
                    break;
                case 1:
                    if (list != null && list.Count() > 0)
                    {
                        list = list
                            .Where(x => x.ReportDays != null && x.ReportDays.Count() > 0 && x.ReportDays.Where(f => f.Logs.Count() > 0).Count() > 0).ToList();
                    }
                    break;
                case 2:
                    if (list != null && list.Count() > 0)
                    {
                        list = list.Where(x => x.ReportDays == null || x.ReportDays.Count() == 0).ToList();
                        list = list.Where(x => x.ReportDays.Where(f => f.Logs.Where(m => m.Remove == false).ToList().Count() == 0).Count() == 0).ToList();
                    }
                    break;
            }

            return list.GetRange(start, Math.Min(lenght, list.Count - start));
        }


        // type = 0  ---> برای کل گزارش های امروز
        // type = 1  ---> برای کل گزارش های حاضرین امروز
        // type = 2  ---> برای کل گزارش های غایبین امروز
        public int InstantCount(string search, int type = 0, string userId = null, int usergroupId = 0)
        {
            var currentDateTime = DateTime.Now;
            var list = GetList;
            if (list != null && list.Count() > 0)
            {
                list = list.OrderBy(x => x.ReportDays != null).ToList();
            }
            else
                list = new List<wskhUser>();

            if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(userId))
                list = list.Where(x => x.Id.Contains(userId)).ToList();

            if (list != null && list.Count() > 0 && usergroupId > 0)
                list = list.Where(x => x.UserGroupId == usergroupId).ToList();

            if (list != null && list.Count() > 0)
            {
                List<wskhUser> newList = new List<wskhUser>();
                foreach (var user in list)
                {
                    if (user.ReportDays != null && user.ReportDays.Count() > 0)
                    {
                        user.ReportDays = user.ReportDays.Where(x => x.ReportDate.Date == currentDateTime.Date).ToList();
                        foreach (var item in user.ReportDays)
                        {
                            item.Logs = item.Logs.Where(x => x.Remove == false).ToList();
                        }
                    }
                    else
                    {
                        user.ReportDays = new List<ReportDay>();
                    }
                }
            }


            switch (type)
            {
                case 0:
                default:
                    if (list != null && list.Count() > 0)
                    {
                        list = list.OrderByDescending(x =>
                        x.ReportDays != null &&
                        x.ReportDays.Count() >= 0)
                            .ToList();
                    }
                    break;
                case 1:
                    if (list != null && list.Count() > 0)
                    {
                        list = list
                            .Where(x => x.ReportDays != null && x.ReportDays.Count() > 0 && x.ReportDays.Where(f => f.Logs.Count() > 0).Count() > 0).ToList();
                    }
                    break;
                case 2:
                    if (list != null && list.Count() > 0)
                    {
                        list = list.Where(x => x.ReportDays == null || x.ReportDays.Count() == 0).ToList();
                        list = list.Where(x => x.ReportDays.Where(f => f.Logs.Where(m => m.Remove == false).ToList().Count() == 0).Count() == 0).ToList();
                    }
                    break;
            }

            return list.Count();
        }

        #endregion
    }
}
