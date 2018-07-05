using System.Collections.Generic;
using  System.Text;

namespace qcloudsms_csharp.httpclient
{
    public class HTTPRequest
    {
        public HTTPMethod method { get; set; }
        public string url { get; set; }
        public string body;

        public Encoding bodyEncoding;
        public Dictionary<string, string> headers;
        public Dictionary<string, string> parameters;
        public int connectTimeout;
        public int requestTimeout;


        public HTTPRequest(HTTPMethod method, string url)
        {
            this.method = method;
            this.url = url;
            this.headers = new Dictionary<string, string>();
            this.parameters = new Dictionary<string, string>();
            this.bodyEncoding = System.Text.Encoding.UTF8;
        }

        public HTTPRequest setBody(string body)
        {
            this.body = body;
            return this;
        }

        public HTTPRequest setBodyEncoding(Encoding bodyEncoding)
        {
            this.bodyEncoding = bodyEncoding;
            return this;
        }

        public HTTPRequest addHeader(string name, string value)
        {
            headers.Add(name, value);
            return this;
        }

        public HTTPRequest addQueryParameter(string name, string value)
        {
            parameters.Add(name, value);
            return this;
        }

        public HTTPRequest addQueryParameter(string name, int value)
        {
            parameters.Add(name, value.ToString());
            return this;
        }

        public HTTPRequest addQueryParameter(string name, long value)
        {
            // TODO fix enum value name
            parameters.Add(name, value.ToString());
            return this;
        }

        public HTTPRequest setConnectionTimeout(int connectionTimeout)
        {
            this.connectTimeout = connectionTimeout;
            return this;
        }

        public HTTPRequest setRequestTimeout(int requestTimeout)
        {
            this.requestTimeout = requestTimeout;
            return this;
        }

        public override string ToString()
        {
            return string.Format("url: {0}, headers: {1}, body: {2}",
                url, headers.ToString(), body);
        }
    }
}