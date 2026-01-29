using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.Model
{
    public class FullLogModel
    {
        public FullLogModel()
        {

        }

        public int Id { get; set; }
        public int? Index { get; set; }
        public string VerifyMode { get; set; }
        public string InOutMode { get; set; }
        public string WorkCode { get; set; }
        public bool Orgin { get; set; }
        public DateTime LogDate { get; set; }
        public string LogTime { get; set; }
        public int? State { get; set; }
        public int? TransportType { get; set; }
        public int? ReportDayId { get; set; }
        public int? EnrollId { get; set; }
        public int? DeviceId { get; set; }
        public int? EnrollNo { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int? Privileg { get; set; }
        public bool Enabled { get; set; }
        public int? FingerDeviceId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string ModelName { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string NationalCode { get; set; }
        public bool Active { get; set; }
        public bool Sex { get; set; }



        public string DayName { get; set; }
        public string LogDatePersian { get; set; }
        public string DeviceName { get; set; }
        public string UserName { get; set; }
        public string StateString { get; set; }
    }

    public class FullAnalayzedModel
    {
        public FullAnalayzedModel()
        {
            FullLogModels = new List<FullLogModel>();
        }
        public string Logs { get; set; }
        public List<FullLogModel> FullLogModels { get; set; }
    }
}
