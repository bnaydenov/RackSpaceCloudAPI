using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace RackSpaceCloudServersAPI
{

    internal class RackSpaceCloudRequest
    {
        private readonly string _baseUrl;
        private readonly string _authToken;

        internal RackSpaceCloudRequest(string baseUrl, string authToken)
        {
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentNullException("baseUrl");

            if (string.IsNullOrEmpty(authToken))
                throw new ArgumentNullException("authToken");

            _baseUrl = baseUrl;
            _authToken = authToken;
        }

        internal dynamic Request(string method, string serviceUrl, ExpandoObject postData)
        {
            if (string.IsNullOrEmpty(serviceUrl))
                throw new ArgumentNullException("serviceUrl");

            string url = string.Format("{0}{1}", _baseUrl, serviceUrl);

            WebRequest request = WebRequest.Create(url);

            request.Method = method;
            request.ContentType = "application/json";
            request.Headers["X-Auth-Token"] = _authToken;

            //Write post data
            if (method == "POST" && postData != null)
            {
                string json = postData.ToString();
                var bytes = Encoding.ASCII.GetBytes(json);
                request.ContentLength = bytes.Length;
                var requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }

            string response;

            using (var webResponse = request.GetResponse())
            {
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
            }

            return response.ToExpando();
        }

        internal dynamic Request(string method, string serviceUrl)
        {
            return Request(method, serviceUrl, null);
        }

        internal dynamic GetRequest(string serviceUrl)
        {
            return Request("GET", serviceUrl, null);
        }
    }
}
