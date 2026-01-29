using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TimeAttendance.Model
{
    public class DeviceCardModel
    {
        public DeviceCardModel()
        {
            TypeList = new List<SelectListItem>();
            TypeList.Add(new SelectListItem() {
                Text = "ورود",
                Value = "0"
            });
            TypeList.Add(new SelectListItem()
            {
                Text = "خروج",
                Value = "1"
            });
            TypeList.Add(new SelectListItem()
            {
                Text = "مرخصی ساعتی",
                Value = "2"
            });
            TypeList.Add(new SelectListItem()
            {
                Text = "ماموریت ساعتی",
                Value = "3"
            });
            TypeList.Add(new SelectListItem()
            {
                Text = "تاخیر سرویس",
                Value = "4"
            });
            TypeList.Add(new SelectListItem()
            {
                Text = "نماز، نهار، استراحت",
                Value = "5"
            });
            TypeList.Add(new SelectListItem()
            {
                Text = "پاس شیر",
                Value = "6"
            });
            TypeList.Add(new SelectListItem()
            {
                Text = "غیره",
                Value = "7"
            });
        }

        public int Id { get; set; }

        public int EnrollNo { get; set; }
        public int ListId { get; set; }
        public List<SelectListItem> TypeList { get; set; }
    }
}
