using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

                    result.Add(new RackSpaceCloudServer
                    {
                        id = server.id,
                        name = server.name,
                        imageId = server.imageId,
                        flavorId = server.flavorId,
                        hostId = server.hostId,
                        status = server.status,
                        progress = server.progress,
                        addresses = new RackSpaceCloudServerIPAdress { privateIP = tempPrivateIP, publicIP = tempPublicIP },
                    });
                }
                else
                {
                    result.Add(new RackSpaceCloudServer { id = server.id, name = server.name });                    
                }
            }

            
            return result;
        }

    }
}
