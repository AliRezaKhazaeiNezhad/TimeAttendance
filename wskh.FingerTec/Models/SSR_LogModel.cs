using System;

namespace wskh.FingerTec.Models
{
    /// <summary>
    /// Log entity
    /// </summary>
    public class SSR_LogModel
    {
        #region Ctor
        public SSR_LogModel()
        {

        }
        #endregion
        #region Propertices
        public string EnrollNo { get; set; }
        public DateTime DateTime { get; set; }
        public int VerifyMode { get; set; }
        public int InOutMode { get; set; }
        public int WorkCode { get; set; }
        #endregion
    }
}
