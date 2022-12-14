using System.ComponentModel.DataAnnotations;

namespace ThuVienOnline.Models
{
    public class TheLoai
    {
        [Key]
        public int TheLoaiID { get; set; }
        public string TheLoaiName { get; set; }
    }
}
