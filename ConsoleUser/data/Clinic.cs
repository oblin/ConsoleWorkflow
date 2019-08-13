using System.ComponentModel.DataAnnotations.Schema;

namespace AbhCare.Identity.Data
{
    [Table("Clinic")]
    public class Clinic
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// TODO: 是否有需要分開 Code & ItemCode => 對應 CodeDetail？
        /// 目前設定是一致的，看起來應該沒有必要分開
        /// </summary>
        [NotMapped]
        public string ItemCode => Code;

        [NotMapped]
        public string CodeDesc => Name;
    }
}
