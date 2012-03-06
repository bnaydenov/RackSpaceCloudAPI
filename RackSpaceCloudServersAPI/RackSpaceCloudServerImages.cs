using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RackSpaceCloudServersAPI
{
    public class RackSpaceCloudServerImage
    {
        public int id { get; set; }
        public string name { get; set; }
        public string updated { get; set; }
        public string created { get; set; }
        public string status { get; set; }
        public int? serverId { get; set; }
        public int? progress { get; set; }
    }
}
