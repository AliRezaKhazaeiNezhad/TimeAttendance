using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Model;

namespace TimeAttendance.Model
{
    public class AboutSoftwareModel : BaseModel
    {
        public AboutSoftwareModel()
        {

        }


        /// <summary>
        /// ورژن
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// تاریخ پابلیش
        /// </summary>
        public string PublishDateTime { get; set; }

        /// <summary>
        /// شرح تغییرات
        /// </summary>
        public string Description { get; set; }


    }
}
