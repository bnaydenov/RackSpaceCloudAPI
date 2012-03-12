using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RackSpaceCloudServersAPI;
using System.Collections;
using System.Configuration;

namespace ConsoleClient
{
    class ConsoleClient
    {
        static void Main(string[] args)
        {
            string rackSpaceCloudUserName = ConfigurationManager.AppSettings["RackSpaceCloudUserName"];
            string rackSpaceCloudAPIKey = ConfigurationManager.AppSettings["RackSpaceCloudAPI"];
            string rackSpaceCloudAuthManagementURL = ConfigurationManager.AppSettings["RackSpaceCloudAuthManagementURL"];
            
            StringBuilder usageSyntax = new StringBuilder();
            usageSyntax.Append("There was an error while parsing the arguments, please check usage syntax below\n\r\n\r");
            usageSyntax.Append("Examples:\n\r\n\r");
            usageSyntax.Append("To list all the RackspaceCloud servers use:\n\r\n\r");
            usageSyntax.Append("CosoleClient.exe -l or CosoleClient.exe -list\n\r\n\r");
            usageSyntax.Append("To get details about given RackspaceCloud servers use:\n\r\n\r");
            usageSyntax.Append("CosoleClient.exe -d=serverId a.k.a CosoleClient.exe -d=12345 \n\r\n\r");
            usageSyntax.Append("To create new RackspaceCloud server use:\n\r\n\r");
            usageSyntax.Append("CosoleClient.exe -c=ServerName \n\r\n\r");
            usageSyntax.Append("To delete RackspaceCloud server use:\n\r\n\r");
            usageSyntax.Append("CosoleClient.exe -delete=serverID a.k.a CosoleClient.exe -delete=12345 \n\r\n\r");

            usageSyntax.Append("NB: Don't forget to add your RackSpaceCloud \"Username\" and \"API key\" in app.config file");

            Hashtable myargs = ArgumentParser.Parse(args);

            if (myargs.ContainsKey("?") || myargs.ContainsKey("Invalid") || myargs.Count < 1)
            {
                Console.WriteLine(usageSyntax);
            }
            else
            {
                //-l or -list parameter used
                if (myargs.ContainsKey("l") || myargs.ContainsKey("list"))
                {
                    ListServers(rackSpaceCloudUserName, rackSpaceCloudAPIKey, rackSpaceCloudAuthManagementURL,true);
                }
                //-d={serverId} will display details about server by given id
                //example -d=1234556
                else if (myargs.ContainsKey("d"))
                {
                    GetServerDetails(rackSpaceCloudUserName, rackSpaceCloudAPIKey, rackSpaceCloudAuthManagementURL, myargs["d"].ToString());
                }
                //-c create server 
                else if (myargs.ContainsKey("c"))
                {
                    CreateServer(rackSpaceCloudUserName, rackSpaceCloudAPIKey, rackSpaceCloudAuthManagementURL, myargs["c"].ToString());
                }
                //-delete=serverID delete server
                else if (myargs.ContainsKey("delete"))
                {
                    DeleteServer(rackSpaceCloudUserName, rackSpaceCloudAPIKey, rackSpaceCloudAuthManagementURL,myargs["delete"].ToString());
                }
                //-updateservername=newServerName server name -serverid=serverId
                else if (myargs.ContainsKey("updateservername") && myargs.ContainsKey("serverid"))
                {
                    UpdateServerName(rackSpaceCloudUserName, rackSpaceCloudAPIKey, rackSpaceCloudAuthManagementURL, myargs["serverid"].ToString(), myargs["updateservername"].ToString());
                }
                //-newpwd=newPassword update server password -serverid=serverId
                else if (myargs.ContainsKey("newpwd") && myargs.ContainsKey("serverid"))
                {
                    UpdateServerPass(rackSpaceCloudUserName, rackSpaceCloudAPIKey, rackSpaceCloudAuthManagementURL, myargs["serverid"].ToString(), myargs["newpwd"].ToString());
                }
                
         }

            Console.ReadKey();
        }

        private static void UpdateServerName(string rackSpaceCloudUserName, string rackSpaceCloudAPIKey, string rackSpaceCloudAuthManagementURL,string serverId,string newServerName)
        {
            AuthInfo authToken = GetRackSpaceAuthInfo(rackSpaceCloudUserName, rackSpaceCloudAPIKey, rackSpaceCloudAuthManagementURL);

            RackSpaceCloudServersAPI.RackSpaceCloudServersAPI rackSpaceCloudServersAPI = new RackSpaceCloudServersAPI.RackSpaceCloudServersAPI(authToken);

            rackSpaceCloudServersAPI.UpdateServerName(serverId, newServerName);

            Console.WriteLine("Update servername");
   
        }


        private static void UpdateServerPass(string rackSpaceCloudUserName, string rackSpaceCloudAPIKey, string rackSpaceCloudAuthManagementURL, string serverId, string newPassword)
        {
            AuthInfo authToken = GetRackSpaceAuthInfo(rackSpaceCloudUserName, rackSpaceCloudAPIKey, rackSpaceCloudAuthManagementURL);

            RackSpaceCloudServersAPI.RackSpaceCloudServersAPI rackSpaceCloudServersAPI = new RackSpaceCloudServersAPI.RackSpaceCloudServersAPI(authToken);

            rackSpaceCloudServersAPI.UpdateServerPass(serverId, newPassword);
            Console.WriteLine("Update password");

        }

        private static void DeleteServer(string rackSpaceCloudUserName, string rackSpaceCloudAPIKey, string rackSpaceCloudAuthManagementURL, string serverId)
        {
            AuthInfo authToken = GetRackSpaceAuthInfo(rackSpaceCloudUserName, rackSpaceCloudAPIKey, rackSpaceCloudAuthManagementURL);

            RackSpaceCloudServersAPI.RackSpaceCloudServersAPI rackSpaceCloudServersAPI = new RackSpaceCloudServersAPI.RackSpaceCloudServersAPI(authToken);

            var serverDetails = rackSpaceCloudServersAPI.DeleteServer(serverId);

            if (serverDetails)
            {
                Console.WriteLine("Deleting server with id: " + serverId + " was successfull!!");
            }
            else
            {
                Console.WriteLine("ERROR!!! Deleting server with id: " + serverId);
            }
            
        }


        private static void CreateServer(string rackSpaceCloudUserName, string rackSpaceCloudAPIKey, string rackSpaceCloudAuthManagementURL, string serverName)
        {
            
            Dictionary<string,string> metadata = new Dictionary<string, string>();
            metadata.Add("key", "value");

            List<Personality> personality = new List<Personality>();
            personality.Add(new Personality { path="",contents=""});
            
            AuthInfo authToken = GetRackSpaceAuthInfo(rackSpaceCloudUserName, rackSpaceCloudAPIKey, rackSpaceCloudAuthManagementURL);

            RackSpaceCloudServersAPI.RackSpaceCloudServersAPI rackSpaceCloudServersAPI = new RackSpaceCloudServersAPI.RackSpaceCloudServersAPI(authToken);

            //imageId=118 -> CentOS 6; 
            //imageId=112 -> Ubuntu 10.0.4 LTS; 
            var serverDetails = rackSpaceCloudServersAPI.CreateServer(serverName, 118, RackSpaceCloudServersAPI.RackSpaceCloudServerFlavor.RAM256, metadata, personality);
            PrintServerDetails(serverDetails);
            
            //get available images to crete new VM
            //var listImages = rackSpaceCloudServersAPI.ListImages();

        }
        
        
        private static void GetServerDetails(string rackSpaceCloudUserName, string rackSpaceCloudAPIKey, string rackSpaceCloudAuthManagementURL, string serverId)
        {
            AuthInfo authToken = GetRackSpaceAuthInfo(rackSpaceCloudUserName, rackSpaceCloudAPIKey, rackSpaceCloudAuthManagementURL);

            RackSpaceCloudServersAPI.RackSpaceCloudServersAPI rackSpaceCloudServersAPI = new RackSpaceCloudServersAPI.RackSpaceCloudServersAPI(authToken);

            var serverDetails = rackSpaceCloudServersAPI.GetServerDetails(serverId);
            PrintServerDetails(serverDetails);

        }

        
        private static void ListServers(string rackSpaceCloudUserName, string rackSpaceCloudAPIKey, string rackSpaceCloudAuthManagementURL, bool details = true)
        {
            AuthInfo authToken = GetRackSpaceAuthInfo(rackSpaceCloudUserName, rackSpaceCloudAPIKey, rackSpaceCloudAuthManagementURL);

            RackSpaceCloudServersAPI.RackSpaceCloudServersAPI rackSpaceCloudServersAPI = new RackSpaceCloudServersAPI.RackSpaceCloudServersAPI(authToken);

            foreach (var server in rackSpaceCloudServersAPI.ListServers(details))
            {
                if (!details)
                {
                    Console.WriteLine(string.Format("Id: {0}", server.id));
                    Console.WriteLine(string.Format("Name: {0}", server.name));
                    Console.WriteLine("****************************************************************");
                }
                else
                {
                    PrintServerDetails(server);
                }
            }
        }

        private static void PrintServerDetails(RackSpaceCloudServer server)
        {
            Console.WriteLine(string.Format("Id: {0}", server.id));
            Console.WriteLine(string.Format("Name: {0}", server.name));
            Console.WriteLine(string.Format("imageId: {0}", server.imageId));
            Console.WriteLine(string.Format("flavorId: {0}", server.flavorId));
            Console.WriteLine(string.Format("serverId: {0}", server.hostId));
            Console.WriteLine(string.Format("status: {0}", server.status));
            Console.WriteLine(string.Format("progress: {0}", server.progress));

            foreach (var ip in server.addresses.privateIP)
            {
                Console.WriteLine(string.Format("addresses privateIP: {0}", ip));
            }
            foreach (var ip in server.addresses.publicIP)
            {
                Console.WriteLine(string.Format("addresses publicIP: {0}", ip));
            }
            Console.WriteLine("****************************************************************");
        }

        
        private static AuthInfo GetRackSpaceAuthInfo(string rackSpaceCloudUserName, string rackSpaceCloudAPIKey, string rackSpaceCloudAuthManagementURL)
        {
            AuthInfo authToken;

            var rackSpaceCloudSecurity = new RackSpaceCloudSecurity(rackSpaceCloudAuthManagementURL);
            authToken = rackSpaceCloudSecurity.AuthenicationRequest(rackSpaceCloudUserName, rackSpaceCloudAPIKey);
            return authToken;
        }
    }
}

