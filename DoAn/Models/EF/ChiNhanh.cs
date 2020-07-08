namespace DoAn.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiNhanh")]
    public partial class ChiNhanh
    {
        public int Id { get; set; }

        public string TenChiNhanh { get; set; }

        public string DiaChi { get; set; }

        [StringLength(50)]
        public string Lat { get; set; }

        [StringLength(50)]
        public string Lng { get; set; }

        public string GhiChu { get; set; }
    }
}
