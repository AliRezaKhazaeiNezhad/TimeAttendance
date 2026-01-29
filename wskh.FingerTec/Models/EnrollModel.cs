namespace wskh.FingerTec.Models
{
    /// <summary>
    /// Model for enrol 
    /// </summary>
    public class EnrollModel
    {
        #region Ctor
        public EnrollModel()
        {

        }
        #endregion

        #region Propertices
        public int Index { get; set; }
        public int EnrollNo { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }
        public int Privilage { get; set; }
        public bool Enable { get; set; }
        public int? tZprg { get; set; }
        public string tZstr { get; set; }
        #endregion
    }
}
