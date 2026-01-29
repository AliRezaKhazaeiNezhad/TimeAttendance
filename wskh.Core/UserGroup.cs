using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;

namespace TimeAttendance.Core
{
    [Table("UserGroups")]
    public class UserGroup : BaseEntity
    {
        #region Ctor
        public UserGroup()
        {
            Users = new List<wskhUser>();
            UserGroupCalendares = new List<UserGroupCalendare>();
        }
        #endregion
        #region Propertices
        [MaxLength(75)]
        public string Title { get; set; }
        #endregion
        #region Relations
        public virtual List<wskhUser> Users { get; set; }
        public virtual List<UserGroupCalendare> UserGroupCalendares { get; set; }
        #endregion
    }
}
