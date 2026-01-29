using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;

namespace TimeAttendance.Core
{
    public class OrganizationInformation : BaseEntity
    {
        public OrganizationInformation()
        {

        }

        [MaxLength(75)]
        public string Title { get; set; }

        [MaxLength(75)]
        public string Category { get; set; }

        [MaxLength(150)]
        public string Address { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [MaxLength(300)]
        public string LogoPath { get; set; }

        public bool Completed { get; set; }
    }
}
