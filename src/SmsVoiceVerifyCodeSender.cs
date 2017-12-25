using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

using System;


namespace qcloudsms_csharp
{
    public class SmsVoiceVerifyCodeSender : SmsBase
    {
        private String url = "https://yun.tim.qq.com/v5/tlsvoicesvr/sendvoice";

        public SmsVoiceVerifyCodeSender(int appid, String appkey)
            : base(appid, appkey, new DefaultHTTPClient())
        { }

        public SmsVoiceVerifyCodeSender(int appid, String appkey, IHTTPClient httpclient)
            : base(appid, appkey, httpclient)
        { }

        /// <summary>
        /// Send a voice verify code message.
        /// </summary>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="phoneNumber">phone number</param>
        /// <param name="msg">voice verify code message</param>
        /// <param name="playtimes">playtimes, optional, max is 3, default is 2</param>
        /// <param name="ext">ext field, content will be returned by server as it is</param>
        /// <returns></returns>
        public SmsVoiceVerifyCodeSenderResult send(String nationCode, String phoneNumber, String msg,
            int playtimes, String ext)
        {
            long random = SmsSenderUtil.getRandom();
            long now = SmsSenderUtil.getCurrentTime();
            JSONObjectBuilder body = new JSONObjectBuilder();
            body.Put("tel", (new JSONObjectBuilder()).Put("nationcode", nationCode).Put("mobile", phoneNumber).Build())
                .Put("msg", msg)
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

            SmsVoiceVerifyCodeSenderResult result = new SmsVoiceVerifyCodeSenderResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);

            return result;
        }
    }
}