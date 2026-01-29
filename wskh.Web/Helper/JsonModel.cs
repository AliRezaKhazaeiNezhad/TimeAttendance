using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wskh.Web.Helper
{
    public class JsonModel
    {
        public JsonModel()
        {
            Exception();
        }

        public string Html { get; set; }
        public string Message { get; set; }
        public string State { get; set; }

        public void Success()
        {
            State = "0";
            Message = "عملیات با موفقیت انجام شد";
        }
        public void Exception()
        {
            State = "1";
            Message = "خطایی رخ داده است! درفرصتی دیگر تلاش نمایید";
        }
        public void Other(string error)
        {
            State = "2";
            Message = error;
        }
    }
}