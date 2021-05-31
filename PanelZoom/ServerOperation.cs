using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PanelZoom
{
    class ServerOperation
    {
        private string server;
        private string contentType;
        private string method;

        private WebRequest webRequest;
        private StreamWriter streamWriter;

        public ServerOperation(string URL, string ContentType, string Method)
        {
            server = URL;
            contentType = ContentType;
            method = Method;
        }
        public void ServerRequest()
        {
            webRequest = WebRequest.Create(server);
            webRequest.ContentType = contentType;
            webRequest.Method = method;
        }

        public void ServerWriter(string text)
        {
            streamWriter = new StreamWriter(webRequest.GetRequestStream());

            streamWriter.Write(text);
            streamWriter.Flush();
        }

        public JObject ServerReader()
        {
            JObject response;

            var httpResponse = (HttpWebResponse)webRequest.GetResponse();

            StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream());

            string result = streamReader.ReadToEnd();

            response = JObject.Parse(result);

            return response;
        }
    }
}
