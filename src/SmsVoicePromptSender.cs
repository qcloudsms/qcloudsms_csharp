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

        /**
         * 发送语音短信
         *
         * @param nationCode 国家码，如 86 为中国
         * @param phoneNumber 不带国家码的手机号
         * @param prompttype 类型，目前固定值为2
         * @param playtimes 播放次数
         * @param msg 语音通知消息内容
         * @param ext  "扩展字段，原样返回"
         * @return {@link}SmsVoicePromptSenderResult
         * @throws HTTPException  http status exception
         * @throws JSONException  json parse exception
         * @throws HttpRequestException    network problem
         */
        public SmsVoicePromptSenderResult send(string nationCode, string phoneNumber, int prompttype,
            int playtimes, string msg, string ext)
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