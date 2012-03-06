using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;


namespace RackSpaceCloudServersAPI
{
    public class RackSpaceCloudServersAPI
    {
        private AuthInfo _authInfo;
        
        public RackSpaceCloudServersAPI(AuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        public List<RackSpaceCloudServer> ListServers(bool details = false)
        {
            List<RackSpaceCloudServer> result = new List<RackSpaceCloudServer>();
            
            var request = new RackSpaceCloudRequest(this._authInfo.ServerManagementUrl, this._authInfo.AuthToken);

            dynamic serverList;
            
            if (details)
            {
                serverList = request.GetRequest("/servers/detail");
            }
            else
            {
                serverList = request.GetRequest("/servers");
            }

            
            foreach (var server in serverList.servers)
            {
                if (details)
                {
                    result.Add(ExpandoToRackSpaceCloudServerObject(server));
                }
                else
                {
                    result.Add(new RackSpaceCloudServer { id = server.id, name = server.name });                    
                }
            }

            
            return result;
        }


        public RackSpaceCloudServer GetServerDetails(string serverId)
        {

            var request = new RackSpaceCloudRequest(this._authInfo.ServerManagementUrl, this._authInfo.AuthToken);

            var serverDetails = request.GetRequest(String.Format("/servers/{0}",serverId));

            var server = ExpandoToRackSpaceCloudServerObject(serverDetails.server);

            return server;
        }


        public RackSpaceCloudServer CreateServer(string serverName, int imageId, RackSpaceCloudServerFlavor flavorId, Dictionary<string, string> metadata = null, List<Personality> personality = null)
        {

            var request = new RackSpaceCloudRequest(this._authInfo.ServerManagementUrl, this._authInfo.AuthToken);
            dynamic data = new ExpandoObject();
            data.server = new ExpandoObject();
            data.server.name = serverName;
            data.server.imageId = imageId;
            data.server.flavorId = flavorId;
            data.server.metadata = metadata;      
            data.server.personality = personality;
            
            
            dynamic response = request.Request("POST", "/servers", data);

            return ExpandoToRackSpaceCloudServerObject(response.server);
        }


        public List<string> ListImages()
        {
            
            List<string> listImages = new List<string>();
            
            RackSpaceCloudServer server = new RackSpaceCloudServer();

            var request = new RackSpaceCloudRequest(this._authInfo.ServerManagementUrl, this._authInfo.AuthToken);


            var images = request.GetRequest("/images");

            foreach (var image in images.images)
            {
                listImages.Add(string.Format("Id: {0}", image.id));
                listImages.Add(string.Format("Name: {0}", image.name));
            }

            return listImages;
        }


        private RackSpaceCloudServer ExpandoToRackSpaceCloudServerObject(dynamic server)
        {

            List<string> tempPublicIP = new List<string>();
            List<string> tempPrivateIP = new List<string>();


            if (server.addresses.@public is ICollection<object>)
            {
                foreach (var ip in server.addresses.@public)
                {
                    tempPublicIP.Add(Convert.ToString(ip.Unknown));
                }

            }

            if (server.addresses.@private is ICollection<object>)
            {
                foreach (var ip in server.addresses.@private)
                {
                    tempPrivateIP.Add(Convert.ToString(ip.Unknown));
                }

            }

            var result = new RackSpaceCloudServer
            {
                id = server.id,
                name = server.name,
                imageId = server.imageId,
                flavorId = server.flavorId,
                hostId = server.hostId,
                status = server.status,
                progress = server.progress,
                addresses = new RackSpaceCloudServerIPAdress { privateIP = tempPrivateIP, publicIP = tempPublicIP },
            };

            return result;
        }        

    }


}
