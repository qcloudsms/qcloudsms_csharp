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

        /**
         * 普通群发
         *
         * 明确指定内容，如果有多个签名，请在内容中以【】的方式添加到信息内容中，否则系统将使用默认签名
         *
         * @param type 短信类型，0 为普通短信，1 营销短信
         * @param nationCode 国家码，如 86 为中国
         * @param phoneNumbers 不带国家码的手机号列表
         * @param msg 信息内容，必须与申请的模板格式一致，否则将返回错误
         * @param extend 扩展码，可填空
         * @param ext 服务端原样返回的参数，可填空
         * @return {@link}SmsMultiSenderResult
         */
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

        public SmsMultiSenderResult send(int type, string nationCode, string[] phoneNumbers,
            string msg, string extend, string ext)
        {
            return send(type, nationCode, new List<string>(phoneNumbers),
                        msg, extend, ext);
        }

        /**
         * 指定模板群发
         *
         * @param nationCode 国家码，如 86 为中国
         * @param phoneNumbers 不带国家码的手机号列表
         * @param templateId 模板 id
         * @param params 模板参数列表
         * @param sign 签名，如果填空，系统会使用默认签名
         * @param extend 扩展码，可以填空
         * @param ext 服务端原样返回的参数，可以填空
         * @return {@link}SmsMultiSenderResult
         */
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