using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;

namespace qcloudsms_csharp
{
    public class SmsMultiSender : SmsBase
    {
        private string url = "https://yun.tim.qq.com/v5/tlssmssvr/sendmultisms2";

        public SmsMultiSender(int appid, string appkey)
        : base(appid, appkey, new DefaultHTTPClient())
        { }

        public SmsMultiSender(int appid, string appkey, IHTTPClient httpclient)
        : base(appid, appkey, httpclient)
        { }

        /// <summary>
        /// Send a SMS messages to multiple phones at once.
        /// </summary>
        /// <param name="type">Send a SMS messages to multiple phones at once.</param>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="phoneNumbers">phone number array</param>
        /// <param name="msg">SMS message content</param>
        /// <param name="extend">extend field, default is empty string</param>
        /// <param name="ext">ext field, content will be returned by server as it is</param>
        /// <returns>SmsMultiSenderResult</returns>
        public SmsMultiSenderResult send(int type, string nationCode, List<string> phoneNumbers,
            string msg, string extend, string ext)
        {

            long random = SmsSenderUtil.getRandom();
            long now = SmsSenderUtil.getCurrentTime();
            JSONObjectBuilder body = new JSONObjectBuilder()
                .Put("tel", toTel(nationCode, phoneNumbers))
                .Put("type", type)
                .Put("msg", msg)
                .Put("sig", SmsSenderUtil.calculateSignature(appkey, random, now, phoneNumbers.ToArray()))
                .Put("time", now)
                .Put("extend", !String.IsNullOrEmpty(extend) ? extend : "")
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

            SmsMultiSenderResult result = new SmsMultiSenderResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);

            return result;
        }

        /// <summary>
        /// Send a SMS messages to multiple phones at once.
        /// </summary>
        /// <param name="type">Send a SMS messages to multiple phones at once.</param>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="phoneNumbers">phone number array</param>
        /// <param name="msg">SMS message content</param>
        /// <param name="extend">extend field, default is empty string</param>
        /// <param name="ext">ext field, content will be returned by server as it is</param>
        /// <returns>SmsMultiSenderResult</returns>
        public SmsMultiSenderResult send(int type, string nationCode, string[] phoneNumbers,
            string msg, string extend, string ext)
        {
            return send(type, nationCode, new List<string>(phoneNumbers),
                        msg, extend, ext);
        }

        /// <summary>
        /// Send a SMS messages with template parameters to multiple phones at once.
        /// </summary>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="phoneNumbers">multiple phone numbers</param>
        /// <param name="templateId">template id</param>
        /// <param name="parameters">template parameters</param>
        /// <param name="sign">Sms user sign</param>
        /// <param name="ext">extend field, default is empty string</param>
        /// <returns>SmsMultiSenderResult</returns>
        public SmsMultiSenderResult sendWithParam(string nationCode, List<string> phoneNumbers,
            int templateId, List<string> parameters, string sign, string extend, string ext)
        {
            long random = SmsSenderUtil.getRandom();
            long now = SmsSenderUtil.getCurrentTime();
            JSONObjectBuilder body = new JSONObjectBuilder()
                .PutArray("tel", toTel(nationCode, phoneNumbers))
                .Put("sign", !String.IsNullOrEmpty(sign) ? sign : "")
                .Put("tpl_id", templateId)
                .Put("params", parameters)
                .Put("sig", SmsSenderUtil.calculateSignature(appkey, random, now, phoneNumbers.ToArray()))
                .Put("time", now)
                .Put("extend", !String.IsNullOrEmpty(extend) ? extend : "")
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

            SmsMultiSenderResult result = new SmsMultiSenderResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);

            return result;
        }

        /// <summary>
        /// Send a SMS messages with template parameters to multiple phones at once.
        /// </summary>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="phoneNumbers">multiple phone numbers</param>
        /// <param name="templateId">template id</param>
        /// <param name="parameters">template parameters</param>
        /// <param name="sign">Sms user sign</param>
        /// <param name="ext">extend field, default is empty string</param>
        /// <returns>SmsMultiSenderResult</returns>
        public SmsMultiSenderResult sendWithParam(string nationCode, string[] phoneNumbers,
            int templateId, string[] parameters, string sign, string extend, string ext)
        {
            return sendWithParam(nationCode, new List<string>(phoneNumbers),
                                 templateId, new List<string>(parameters), sign, extend, ext);
        }

        private JArray toTel(string nationCode, List<string> phoneNumbers)
        {
            JSONArrayBuilder builder = new JSONArrayBuilder();
            foreach (var phoneNumber in phoneNumbers)
            {
                JSONObjectBuilder phone = new JSONObjectBuilder();
                phone.Put("nationcode", nationCode);
                phone.Put("mobile", phoneNumber);
                builder.Put(phone.Build());
            }

            return builder.Build();
        }

    }
}