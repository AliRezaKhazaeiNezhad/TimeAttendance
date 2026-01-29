using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.Core
{
    public class AboutSoftware
    {
        #region Ctor
        public AboutSoftware()
        {

        }
        #endregion


        #region Propertices

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// ورژن
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// تاریخ پابلیش
        /// </summary>
        [Column(TypeName = "Date")]
        public DateTime PublishDateTime { get; set; }

        /// <summary>
        /// شرح تغییرات
        /// </summary>
        public string Description { get; set; }


        #endregion


        #region Relations

        #endregion
    }
}
