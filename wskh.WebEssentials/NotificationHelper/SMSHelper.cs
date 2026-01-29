using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.WebEssentials.NotificationHelper
{
    public static class SMSHelper
    {
        #region Send
        public static bool Send(string message)
        {
            try
            {
                WebRequest request = WebRequest.Create($"https://rest.payamak-panel.com/api/SendSMS/SendSMS");
                request.Method = "POST";
                string mobileNumber = "09215272595";
                string postData = "{'username': '9154294491','password': '2412','to': '" + mobileNumber + "','from': '30004657311651', 'text': '" + message + "', 'isFlash': false }";


                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();


                WebResponse response = request.GetResponse();
                var code = (HttpWebResponse)request.GetResponse();
                response.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion
    }
}
