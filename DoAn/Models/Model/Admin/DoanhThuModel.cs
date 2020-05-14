using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DoAn.Models.Model.Admin
{
    [DataContract]
    public class DoanhThuModel
    {
        public DoanhThuModel(string label, double y, string ngayban, int soluong)
        {
            this.Label = label;
            this.Y = y;
            this.NgayBan = ngayban;
            this.SoLuongBan = soluong;

        }
        public DoanhThuModel()
        {
            this.Label = "";
            this.Y = 0;
            this.NgayBan = "";
            this.SoLuongBan = 0;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label { set; get; }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y { set; get; }

        public int SoLuongBan { set; get; }

        public string NgayBan { set; get; }
    }
}