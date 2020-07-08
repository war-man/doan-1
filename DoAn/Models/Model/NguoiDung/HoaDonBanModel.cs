using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Models.Model.NguoiDung
{
    public class HoaDonBanModel

    {
        [Key]
        public string Id { set; get; }
        public int STT { set; get; }

        public int MaNhanVien { set; get; }

        public int? MaKhach { set; get; }

        public DateTime? NgayBan { set; get; }

        [Display(Name = "Mã Giảm Giá")]
        public int MaGiamGia { set; get; }

        
        [Display(Name = "Địa Chỉ")]
        public string DiaChi { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        [Display(Name = "Số Điện Thoại")]
        public string SDT { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập họ tên")]
        [Display(Name = "Họ Tên")]
        public string HoTen { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập email")]
        [Display(Name = "Email")]
        public string Email { set; get; }

        public SelectList SelectChiNhanh { set; get; }

        
        [Display(Name = "Chi Nhánh")]
        public int MaChiNhanh { set; get; }

        [Display(Name = "Tổng Tiền")]
        public int? TongTien { set; get; }

        public int? KhuyenMai { set; get; }

        public int? DaThanhToan { set; get; }

        public int? Status { set; get; }

        public string TenDangNhap { set; get; }

        public int? TongTienDaMua { set; get; }

        public int? TongSoHoaDon { set; get; }
        public int? TongSoSanPham { set; get; }

        [Display(Name = "Phí ship")]
        public int? PhiShip { set; get; }

        public int? TongTienHoaDon { set; get; }
    }
}