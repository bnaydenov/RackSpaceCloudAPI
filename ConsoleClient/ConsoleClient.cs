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

                
            }

            Console.ReadKey();
        }

        
        
        private static void ListServers(string rackSpaceCloudUserName, string rackSpaceCloudAPIKey, string rackSpaceCloudAuthManagementURL, bool details = true)
        {
            AuthInfo authToken;

            var rackSpaceCloudSecurity = new RackSpaceCloudSecurity(rackSpaceCloudAuthManagementURL);
            authToken = rackSpaceCloudSecurity.AuthenicationRequest(rackSpaceCloudUserName, rackSpaceCloudAPIKey);

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
                    Console.WriteLine(string.Format("Id: {0}", server.id));
                    Console.WriteLine(string.Format("Name: {0}", server.name));
                    Console.WriteLine(string.Format("imageId: {0}", server.imageId));
                    Console.WriteLine(string.Format("flavorId: {0}", server.flavorId));
                    Console.WriteLine(string.Format("hostId: {0}", server.hostId));
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
            }
        }
    }
}

