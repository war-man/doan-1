using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Models.Model.Admin
{
    public class SanPhamModel
    {
        [Key]
        public int? Id { set; get; }
        public int STT { set; get; }

        [Required(ErrorMessage = "Mời nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string Ten { set; get; }

        [Required(ErrorMessage = "Mời chọn tên loại")]
        [Display(Name = "Loại sản phẩm")]
        public int? MaLoai { set; get; }



        public string TenLoai { set; get; }

        [Required(ErrorMessage = "Mời nhập giá bán")]
        [Display(Name = "Giá bán")]
        public string GiaBan { set; get; }

        public int? KhuyenMai { set; get; }
        public string KhuyenMaiShow { set; get; }

        [Required(ErrorMessage = "Mời chọn ảnh")]
        [Display(Name = "Hình ảnh")]
        public string Anh { set; get; }


        [Display(Name = "Mô tả")]
        public string MoTa { set; get; }

        public SelectList SelectMaLoai { set; get; }

        public int? TongSL { set; get; }

        public string PhanTramKM { set; get; }
    }
}