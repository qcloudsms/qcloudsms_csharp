using System.Collections.Generic;


namespace qcloudsms_csharp.httpclient
{
    public class HTTPResponse
    {
        public HTTPRequest request;
        public int statusCode;
        public string reason;
        public string body;
        public Dictionary<string, List<string>> headers;

        public HTTPResponse()
        {
            this.headers = new Dictionary<string, List<string>>();
        }

        public HTTPResponse(int statusCode) : this()
        {
            this.statusCode = statusCode;
        }

        public HTTPResponse(int statusCode, string body) : this()
        {
            this.statusCode = statusCode;
            this.body = body;
        }

        public HTTPResponse(int statusCode, string body, string reason) : this()
        {
            this.statusCode = statusCode;
            this.body = body;
            this.reason = reason;
        }

        public HTTPResponse setStatusCode(int statusCode)
        {
            this.statusCode = statusCode;
            return this;
        }

        public HTTPResponse setBody(string body)
        {
            this.body = body;
            return this;
        }

        public HTTPResponse setReason(string reason)
        {
            this.reason = reason;
            return this;
        }

        public HTTPResponse addHeader(string name, string value)
        {
            if (!headers.ContainsKey(name)) {
                headers.Add(name, new List<string>{value});
            } else {
                headers[name].Add(value);
            }
            return this;
        }

        public HTTPResponse setRequest(HTTPRequest request)
        {
            this.request = request;
            return this;
        }
    }
}