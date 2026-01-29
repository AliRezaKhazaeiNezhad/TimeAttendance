using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.Core
{
    public class BaseEntity
    {
        public BaseEntity()
        {

        }

        /// <summary>
        /// ای دی 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// حذف / عدم نمایش
        /// </summary>
        public bool Remove { get; set; }
    }
}
