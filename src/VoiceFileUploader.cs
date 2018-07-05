using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

using System;
using System.Text;


namespace qcloudsms_csharp
{
    public class VoiceFileUploader : SmsBase
    {
        public enum ContentType
        {
            WAV, MP3
        }

        private string url = "https://cloud.tim.qq.com/v5/tlsvoicesvr/uploadvoicefile";

        public VoiceFileUploader(int appid, string appkey) : base(appid, appkey, new DefaultHTTPClient())
        { }

        public VoiceFileUploader(int appid, string appkey, IHTTPClient httpclient) : base(appid, appkey, httpclient)
        { }

        /// <summary>
        /// Send a file voice.
        /// </summary>
        /// <param name="fileContent">file content bytes</param>
        /// <param name="contentType">file content type</param>
        /// <returns>VoiceFileUploaderResult</returns>
        public VoiceFileUploaderResult upload(byte[] fileContent, ContentType contentType)
        {
            long random = SmsSenderUtil.getRandom();
            long now = SmsSenderUtil.getCurrentTime();
            string fileSha1Sum = SmsSenderUtil.sha1sum(fileContent);
            string auth = SmsSenderUtil.calculateAuth(this.appkey, random, now, fileSha1Sum);;

            string type;
            if (contentType == ContentType.WAV)
            {
                type = "audio/wav";
            }
            else
            {
                type = "audio/mpeg";
            }

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            HTTPRequest req = new HTTPRequest(HTTPMethod.POST, this.url)
                .addHeader("Content-Type", type)
                .addHeader("x-content-sha1", fileSha1Sum)
                .addHeader("Authorization", auth)
                .addQueryParameter("sdkappid", this.appid)
                .addQueryParameter("random", random)
                .addQueryParameter("time", now)
                .setConnectionTimeout(60 * 1000)
                .setRequestTimeout(60 * 1000)
                .setBody(iso.GetString(fileContent))
                .setBodyEncoding(iso);

            // May throw HttpRequestException
            HTTPResponse res = httpclient.fetch(req);

            // May throw HTTPException
            handleError(res);

            VoiceFileUploaderResult result = new VoiceFileUploaderResult();
            // May throw JSONException
            result.parseFromHTTPResponse(res);

            return result;
        }
    }
}