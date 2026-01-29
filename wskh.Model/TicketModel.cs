using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core.Enumerator;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class TicketModel : BaseModel
    {
        public TicketModel()
        {

        }


        #region Propertices
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(50)]
        public string Title { get; set; }

        [Display(Name = "شرح")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(500)]
        public string Description { get; set; }

        [Display(Name = "پاسخ")]
        [MaxLength(500)]
        public string Response { get; set; }
        public TicketState State { get; set; }


        [Display(Name = "وضعیت")]
        public string StateString { get; set; }
        public string Buttom { get; set; }
        #endregion
    }
}
