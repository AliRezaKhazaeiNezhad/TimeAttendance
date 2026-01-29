namespace wskh.FingerTec.Models
{
    /// <summary>
    /// Model for enrol 
    /// </summary>
    public class SSR_EnrollModel
    {
        #region Ctor
        public SSR_EnrollModel()
        {

        }
        #endregion

        #region Propertices
        public string EnrollNo { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }
        public int Privilage { get; set; }
        public bool Enable { get; set; }
        #endregion
    }
}
