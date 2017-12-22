using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;


namespace qcloudsms_csharp
{
    public class SmsStatusPullCallbackResult : SmsResultBase
    {
        public class Callback
        {
            public string user_receive_time;
            public string nationcode;
            public string mobile;
            public string report_status;
            public string errmsg;
            public string description;
            public string sid;

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }

            public Callback parse(JObject json)
            {
                try
                {
                    user_receive_time = json.GetValue("user_receive_time").Value<string>();
                    nationcode = json.GetValue("nationcode").Value<string>();
                    mobile = json.GetValue("mobile").Value<string>();
                    report_status = json.GetValue("report_status").Value<string>();
                    errmsg = json.GetValue("errmsg").Value<string>();
                    description = json.GetValue("description").Value<string>();
                    sid = json.GetValue("sid").Value<string>();
                }
                catch (ArgumentNullException e)
                {
                    throw new JSONException(String.Format("json: {0}, exception: {1}", json, e.Message));
                }

                return this;
            }
        }

        public int result;
        public string errMsg;
        public int count;
        public List<Callback> callbacks;

        public SmsStatusPullCallbackResult()
        {
            this.errMsg = "";
            this.count = 0;
            this.callbacks = new List<Callback>();
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

            if (result == 0)
            {
                try
                {
                    count = json.GetValue("count").Value<int>();
                }
                catch (ArgumentNullException e)
                {
                    throw new JSONException(String.Format("res: {0}, exception: {1}", response.body, e.Message));
                }

                if (json["data"] != null)
                {
                    foreach (JObject item in json["data"])
                    {
                        callbacks.Add((new Callback()).parse(item));
                    }
                }
            }
        }
    }
}