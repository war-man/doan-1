using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAn.Models.Model.Admin
{
    public class KhachHangModel
    {
        
        public int Id { get; set; }

        public int STT { set; get; }

        
        public string HoTen { get; set; }

        
        public string SDT { get; set; }
        public string TenDangNhap { get; set; }
        public string Email { set; get; }
        public string DiaChi { set; get; }
    }
}