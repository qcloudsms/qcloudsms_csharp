using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

using System;


namespace qcloudsms_csharp
{
    public class SmsVoicePromptSender : SmsBase
    {
        private string url = "https://yun.tim.qq.com/v5/tlsvoicesvr/sendvoiceprompt";

        public SmsVoicePromptSender(int appid, string appkey) : base(appid, appkey, new DefaultHTTPClient())
        { }

        public SmsVoicePromptSender(int appid, string appkey, IHTTPClient httpclient) : base(appid, appkey, httpclient)
        { }

        /// <summary>
        /// Send a voice prompt message.
        /// </summary>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="phoneNumber">phone number</param>
        /// <param name="prompttype">voice prompt type, currently value is 2</param>
        /// <param name="msg">voice prompt message</param>
        /// <param name="playtimes">playtimes, optional, max is 3, default is 2</param>
        /// <param name="ext">ext field, content will be returned by server as it is</param>
        /// <returns>SmsVoicePromptSenderResult</returns>
        public SmsVoicePromptSenderResult send(string nationCode, string phoneNumber, int prompttype,
            string msg, int playtimes, string ext)
        {
            long random = SmsSenderUtil.getRandom();
            long now = SmsSenderUtil.getCurrentTime();
            JSONObjectBuilder body = new JSONObjectBuilder()
                .Put("tel", (new JSONObjectBuilder()).Put("nationcode", nationCode).Put("mobile", phoneNumber).Build())
                .Put("prompttype", prompttype)
                .Put("promptfile", msg)
                .Put("playtimes", playtimes)
                .Put("sig", SmsSenderUtil.calculateSignature(this.appkey, random, now, phoneNumber))
                .Put("time", now)
                .Put("ext", !String.IsNullOrEmpty(ext) ? ext : "");

            HTTPRequest req = new HTTPRequest(HTTPMethod.POST, this.url)
                .addHeader("Conetent-Type", "application/json")
                .addQueryParameter("sdkappid", this.appid)
                .addQueryParameter("random", random)
                .setConnectionTimeout(60 * 1000)
                .setRequestTimeout(60 * 1000)
                .setBody(body.Build().ToString());

            // May throw HttpRequestException
            HTTPResponse res = httpclient.fetch(req);

            // May throw HTTPException
            handleError(res);

            SmsVoicePromptSenderResult result = new SmsVoicePromptSenderResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);

            return result;
        }
    }
}