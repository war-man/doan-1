namespace DoAn.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDonBan")]
    public partial class HoaDonBan
    {
        [StringLength(50)]
        public string Id { get; set; }

        public int? MaNhanVien { get; set; }

        public int? MaKhach { get; set; }

        [StringLength(500)]
        public string DiaChi { get; set; }

        public DateTime? NgayBan { get; set; }

        public int? PhiShip { get; set; }

        public int? TongTien { get; set; }

        public int? TongTien_HoaDon { get; set; }

        public int? DaThanhToan { get; set; }

        public int? Duyet { get; set; }

        public int? MaChiNhanh { get; set; }
    }
}
