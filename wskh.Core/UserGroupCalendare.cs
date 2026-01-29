using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.Core
{
    public class UserGroupCalendare
    {
        public UserGroupCalendare()
        {

        }
        [Key]
        public int Id { get; set; }
        public bool Remove { get; set; }




        public int UserGroupId { get; set; }
        public virtual UserGroup UserGroup { get; set; }


        public int CalendarId { get; set; }
        public virtual Calendar Calendar { get; set; }
    }
}
