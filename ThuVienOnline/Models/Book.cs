﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuVienOnline.Models
{
    public partial class Book
    {
        public int BookId { get; set; }
        //public string TheLoai { get; set; }
        public string BookName { get; set; }
        public string TacGia { get; set; }
        public string Anh { get; set; }
        public DateTime NgayPh { get; set; }
        public int SoTrang { get; set; }
        public string Mota { get; set; }
        public int TheLoaiID { get; set; }
        [ForeignKey("TheLoaiID")]
        public TheLoai? TheLoai { get; set; }
    }

    //public enum TheLoai
    //{
    //Truyện_Tranh,
    //Tài_Liệu,
    //Sách_Giáo_Khoa
        
    //}
}
