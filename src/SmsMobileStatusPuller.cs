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

        /// <summary>
        /// Pull SMS messages status for single mobile.
        /// </summary>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="mobile">mobile number</param>
        /// <param name="beginTime">begin time, unix timestamp</param>
        /// <param name="endTime">begin time, unix timestamp</param>
        /// <param name="max">maximum number of message status</param>
        /// <returns>SmsStatusPullCallbackResult</returns>
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

        /// <summary>
        /// Pull reply SMS message status for single mobile.
        /// </summary>
        /// <param name="nationCode">nation dialing code, e.g. China is 86, USA is 1</param>
        /// <param name="mobile">mobile number</param>
        /// <param name="beginTime">begin time, unix timestamp</param>
        /// <param name="endTime">end time, unix timestamp</param>
        /// <param name="max">maximum number of message status</param>
        /// <returns>SmsStatusPullReplyResult</returns>
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