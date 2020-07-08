﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAn.Models.Model.NguoiDung
{
    public class DoiMatKhauNguoiDung
    {
        [Display(Name = "Mật khẩu cũ")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        public string MatKhauCu { set; get; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 kí tự")]
        public string MatKhau { set; get; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Yêu cầu xác nhận mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        public string XacNhanMatKhau { set; get; }
    }
}