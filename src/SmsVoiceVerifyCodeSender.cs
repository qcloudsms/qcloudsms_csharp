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

        /**
         * 发送语音短信
         *
         * @param nationCode 国家码，如 86 为中国
         * @param phoneNumber 不带国家码的手机号
         * @param msg 消息类型
         * @param playtimes 播放次数
         * @param ext 服务端原样返回的参数，可填空
         * @return {@link}SmsVoiceVerifyCodeSenderResult
         * @throws HTTPException  http status exception
         * @throws JSONException  json parse exception
         * @throws HttpRequestException    network problem
         */
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