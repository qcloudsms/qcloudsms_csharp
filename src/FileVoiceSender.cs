using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

using System;


namespace qcloudsms_csharp
{
    public class FileVoiceSender : SmsBase
    {
        private string url = "https://cloud.tim.qq.com/v5/tlsvoicesvr/sendfvoice";

        public FileVoiceSender(int appid, string appkey) : base(appid, appkey, new DefaultHTTPClient())
        { }

        public FileVoiceSender(int appid, string appkey, IHTTPClient httpclient) : base(appid, appkey, httpclient)
        { }

        /// <summary>
        /// Send a file voice.
        /// </summary>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="phoneNumber">phone number</param>
        /// <param name="fid">voice file fid</param>
        /// <param name="playtimes">playtimes, optional, max is 3, default is 2</param>
        /// <param name="ext">ext field, content will be returned by server as it is</param>
        /// <returns>FileVoiceSenderResult</returns>
        public FileVoiceSenderResult send(string nationCode, string phoneNumber, string fid,
            int playtimes, string ext)
        {
            long random = SmsSenderUtil.getRandom();
            long now = SmsSenderUtil.getCurrentTime();
            JSONObjectBuilder body = new JSONObjectBuilder()
                .Put("tel", (new JSONObjectBuilder()).Put("nationcode", nationCode).Put("mobile", phoneNumber).Build())
                .Put("fid", fid)
                .Put("playtimes", playtimes)
                .Put("sig", SmsSenderUtil.calculateSignature(this.appkey, random, now, phoneNumber))
                .Put("time", now)
                .Put("ext", !String.IsNullOrEmpty(ext) ? ext : "");

            HTTPRequest req = new HTTPRequest(HTTPMethod.POST, this.url)
                .addHeader("Content-Type", "application/json")
                .addQueryParameter("sdkappid", this.appid)
                .addQueryParameter("random", random)
                .setConnectionTimeout(60 * 1000)
                .setRequestTimeout(60 * 1000)
                .setBody(body.Build().ToString());

            // May throw HttpRequestException
            HTTPResponse res = httpclient.fetch(req);

            // May throw HTTPException
            handleError(res);

            FileVoiceSenderResult result = new FileVoiceSenderResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);

            return result;
        }
    }
}