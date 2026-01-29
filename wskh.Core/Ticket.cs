using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;
using wskh.Core.Enumerator;

namespace TimeAttendance.Core
{
    public class Ticket : BaseEntity
    {
        #region Ctor
        public Ticket()
        {

        }
        #endregion


        #region Propertices
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [MaxLength(500)]
        public string Response { get; set; }
        public TicketState State { get; set; }
        #endregion


        #region Relations
        public string RequestUserId { get; set; }
        [ForeignKey("RequestUserId")]
        public virtual wskhUser RequestUser { get; set; }


        public string ResponseUserId { get; set; }
        [ForeignKey("ResponseUserId")]
        public virtual wskhUser ResponseUser { get; set; }
        #endregion
    }
}
