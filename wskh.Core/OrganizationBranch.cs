using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.Core
{
    [Table("OrganizationBranchs")]
    public class OrganizationBranch : BaseEntity
    {
        public OrganizationBranch()
        {
            Users = new List<wskhUser>();
        }

        /// <summary>
        /// عنوان
        /// </summary>
        [MaxLength(75)]
        public string Title { get; set; }



        public virtual List<wskhUser> Users { get; set; }
    }
}
