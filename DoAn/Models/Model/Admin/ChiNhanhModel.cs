using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Model.Admin
{
    public class ChiNhanhModel
    {
        public int Id { set; get; }
        
        public string TenChiNhanh { set; get; }
        
        public string DiaChi { set; get; }
        
        public string GhiChu { set; get; }

        public int STT { set; get; }

        public string Lat { set; get; }

        public string Lng { set; get; }
    }
}