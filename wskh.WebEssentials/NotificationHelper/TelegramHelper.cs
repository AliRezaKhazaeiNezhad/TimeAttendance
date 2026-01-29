using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.WebEssentials.NotificationHelper
{
    public static class Telegram
    {
        #region Send
        public static void Send(string message)
        {
            try
            {

                WebRequest request = WebRequest.Create($"http://telegram.microapi.ir/api/Message/Send");
                request.Method = "POST";
                string chatId = "@khzsoftwarebugs";

                string postData = "{'UserName': 'xnNwvM9*4Exzr+@','PassWord': 'dLm4!N-MrTt-dG&','Token': '848004990:AAHP4uxZTI-pIxJmnykWeVEBZ4rZPmDjAYw','Admin': '" + chatId + "',Message: '" + message + "'}";


                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();


                WebResponse response = request.GetResponse();
                var code = (HttpWebResponse)request.GetResponse();
                response.Close();

            }
            catch (Exception e)
            {
            }
        }
        #endregion
    }
}
