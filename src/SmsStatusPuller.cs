using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

namespace qcloudsms_csharp
{
    public class SmsStatusPuller : SmsBase
    {
        private string url = "https://yun.tim.qq.com/v5/tlssmssvr/pullstatus";

        public SmsStatusPuller(int appid, string appkey)
            : base(appid, appkey, new DefaultHTTPClient())
        { }

        public SmsStatusPuller(int appid, string appkey, IHTTPClient httpclient)
            : base(appid, appkey, httpclient)
        { }

        private HTTPResponse pull(int type, int max)
        {

            long random = SmsSenderUtil.getRandom();
            long now = SmsSenderUtil.getCurrentTime();
            JSONObjectBuilder body = new JSONObjectBuilder()
                .Put("sig", SmsSenderUtil.calculateSignature(this.appkey, random, now))
                .Put("time", now)
                .Put("type", type)
                .Put("max", max);

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
        /// Pull callback SMS messages status.
        /// </summary>
        /// <param name="max">maximum number of message status</param>
        /// <returns>SmsStatusPullCallbackResult</returns>
        public SmsStatusPullCallbackResult pullCallback(int max)
        {
            // May throw HttpRequestException
            HTTPResponse res = pull(0, max);

            SmsStatusPullCallbackResult result = new SmsStatusPullCallbackResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);

            return result;
        }


        /// <summary>
        /// Pull reply SMS messages status.
        /// </summary>
        /// <param name="max">maximum number of message status</param>
        /// <returns>SmsStatusPullReplyResult</returns>
        public SmsStatusPullReplyResult pullReply(int max)
        {

            // May throw HttpRequestException
            HTTPResponse res = pull(1, max);

            SmsStatusPullReplyResult result = new SmsStatusPullReplyResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);

            return result;
        }
    }
}