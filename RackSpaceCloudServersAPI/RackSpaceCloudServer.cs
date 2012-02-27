using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RackSpaceCloudServersAPI
{
    public class RackSpaceCloudServer
    {
        public int id { get; set; }
        public string name { get; set; }
        public int imageId { get; set; }
        public int flavorId { get; set; }
        public string hostId { get; set; }
        public string status { get; set; }
        public int progress { get; set; }
        public RackSpaceCloudServerIPAdress addresses {get;set;}
        //public RackSpaceCloudServerMetaData metadata {get;set;}
    }

    public class RackSpaceCloudServerIPAdress
    {
        public List<string> publicIP {get;set;}
        public List<string> privateIP { get; set; }
    }

    public class RackSpaceCloudServerMetaData
    {
        public string serverLabel {get;set;}
        public string imageVersion {get;set;}
    }
}
