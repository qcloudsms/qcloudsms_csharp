using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;


namespace qcloudsms_csharp
{
    public class SmsMobileStatusPuller : SmsBase
    {
        private string url = "https://yun.tim.qq.com/v5/tlssmssvr/pullstatus4mobile";

        public SmsMobileStatusPuller(int appid, string appkey)
            : base(appid, appkey, new DefaultHTTPClient())
        { }

        public SmsMobileStatusPuller(int appid, string appkey, IHTTPClient httpclient)
            : base(appid, appkey, httpclient)
        { }

        private HTTPResponse pull(int type, string nationCode, string mobile, long beginTime,
            long endTime, int max)
        {
            long random = SmsSenderUtil.getRandom();
            long now = SmsSenderUtil.getCurrentTime();
            JSONObjectBuilder body = new JSONObjectBuilder();
            body.Put("sig", SmsSenderUtil.calculateSignature(this.appkey, random, now))
                .Put("type", type)
                .Put("time", now)
                .Put("max", max)
                .Put("begin_time", beginTime)
                .Put("end_time", endTime)
                .Put("nationcode", nationCode)
                .Put("mobile", mobile);

            HTTPRequest req = new HTTPRequest(HTTPMethod.POST, this.url)
                .addHeader("Conetent-Type", "application/json")
                .addQueryParameter("sdkappid", this.appid)
                .addQueryParameter("random", random)
                .setConnectionTimeout(60 * 1000)
                .setRequestTimeout(60 * 10000)
                .setBody(body.Build().ToString());

            // May throw HttpRequestException
            HTTPResponse res = httpclient.fetch(req);

            // May throw HTTPException
            handleError(res);

            return res;
        }

        public SmsStatusPullCallbackResult pullCallback(string nationCode, string mobile,
            long beginTime, long endTime, int max)
        {
            // May throw HttpRequestException
            HTTPResponse res = pull(0, nationCode, mobile, beginTime, endTime, max);

            SmsStatusPullCallbackResult result = new SmsStatusPullCallbackResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);

            return result;
        }

        public SmsStatusPullReplyResult pullReply(string nationCode, string mobile,
            long beginTime, long endTime, int max)
        {
            // May throw HttpRequestException
            HTTPResponse res = pull(1, nationCode, mobile, beginTime, endTime, max);

            SmsStatusPullReplyResult result = new SmsStatusPullReplyResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);

            return result;
        }
    }
}