using System;
using System.Net;
using RackSpaceCloudServersAPI;


namespace RackSpaceCloudServersAPI
{
    public class RackSpaceCloudSecurity
    {
        private string _url;

        public RackSpaceCloudSecurity(string url)
        {
            _url = url;
        }

        public AuthInfo AuthenicationRequest(string user, string key)
        {
            if (string.IsNullOrEmpty(user))
                throw new ArgumentNullException("user");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            WebRequest request = WebRequest.Create(_url);

            request.Headers["X-Auth-User"] = user;
            request.Headers["X-Auth-Key"] = key;

            AuthInfo info = new AuthInfo();

            using (var response = request.GetResponse())
            {
                info.ServerManagementUrl = response.Headers["X-Server-Management-Url"];
                info.AuthToken = response.Headers["X-Auth-Token"];
            }

            return info;
        }
    }
}
