using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;


namespace qcloudsms_csharp
{
    public class SmsStatusPullReplyResult : SmsResultBase
    {
        public class Reply
        {
            public string nationcode;
            public string mobile;
            public string text;
            public string sign;
            public long time;

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }

            public Reply parse(JObject json)
            {
                try
                {
                    nationcode = json.GetValue("nationcode").Value<string>();
                    mobile = json.GetValue("mobile").Value<string>();
                    text = json.GetValue("text").Value<string>();
                    sign = json.GetValue("sign").Value<string>();
                    time = json.GetValue("time").Value<long>();
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
        public List<Reply> replys;

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
                        replys.Add((new Reply()).parse(item));
                    }
                }
            }
        }
    }
}