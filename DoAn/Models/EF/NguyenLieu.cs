namespace DoAn.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NguyenLieu")]
    public partial class NguyenLieu
    {
        public int Id { get; set; }

        public string TenNguyenLieu { get; set; }

        [StringLength(50)]
        public string DonViTinh { get; set; }

        public int? GiaNhap { get; set; }

        public int? SoLuong { get; set; }
    }
}
