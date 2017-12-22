using System;
using System.Runtime.Serialization;

namespace qcloudsms_csharp.httpclient
{
    public class HTTPException : Exception
    {
        private int statusCode;
        private string reason;

        public HTTPException(int statusCode, string reason)
        {
            this.statusCode = statusCode;
            this.reason = reason;
        }

        public override string ToString()
        {
            return String.Format("HTTP statusCode: {0}, reason: {1}", statusCode, reason);
        }

        public int getStatusCode()
        {
            return statusCode;
        }

        public string getReason()
        {
            return reason;
        }
    }
}