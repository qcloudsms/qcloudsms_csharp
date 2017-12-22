using System;


namespace qcloudsms_csharp.json
{
    public class JSONException : Exception
    {
        public JSONException()
        { }

        public JSONException(string message) : base(message)
        { }
    }
}