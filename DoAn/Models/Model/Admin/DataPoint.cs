using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DoAn.Models.Model.Admin
{
    [DataContract]
    public class DataPoint
    {
        public DataPoint(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }
        public DataPoint()
        {
            this.Label = "";
            this.Y = 0;
        }
        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}