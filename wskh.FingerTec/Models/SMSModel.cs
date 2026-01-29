
namespace wskh.FingerTec.Models
{
    /// <summary>
    /// SMS model
    /// </summary>
    public class SMSModel
    {
        #region Ctor
        public SMSModel()
        {

        }
        #endregion
        #region Propertices
        public int Id { get; set; }
        public int Tage { get; set; }
        public int Minute { get; set; }
        public string StartTime { get; set; }
        public string Content { get; set; }
        #endregion
    }
}
