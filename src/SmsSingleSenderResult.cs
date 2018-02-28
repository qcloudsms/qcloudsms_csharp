using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;


namespace qcloudsms_csharp
{
    public class SmsSingleSenderResult : SmsResultBase
    {
        public int result;
        public string errMsg;
        public string ext;
        public string sid;
        public int fee;

        public SmsSingleSenderResult()
        {
            this.errMsg = "";
            this.ext = "";
            this.sid = "";
            this.fee = 0;
        }

        public override void parseFromHTTPResponse(HTTPResponse response)
        {
            JObject json = parseToJson(response);

            try
            {
                result = json.GetValue("result").Value<int>();
                errMsg = json.GetValue("errmsg").Value<string>();
            }
            catch (ArgumentNullException e)
            {
                throw new JSONException(String.Format("res: {0}, exception: {1}", response.body, e.Message));
            }

            if (json["ext"] != null)
            {
                ext = json.GetValue("ext").Value<String>();
            }
            if (json["sid"] != null)
            {
                sid = json.GetValue("sid").Value<String>();
            }
            if (json["fee"] != null)
            {
                fee = json.GetValue("fee").Value<int>();
            }

        }
    }
}