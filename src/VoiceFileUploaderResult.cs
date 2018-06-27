using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

namespace qcloudsms_csharp
{
    public class VoiceFileUploaderResult : SmsResultBase
    {
        public int result;
        public string errMsg;
        public string fid;

        public VoiceFileUploaderResult()
        {
            this.errMsg = "";
            this.fid = "";
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

            if (json["fid"] != null)
            {
                fid = json.GetValue("fid").Value<String>();
            }
        }
    }
}
