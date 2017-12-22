using System;
using System.Text;
using System.Net.Http;

namespace qcloudsms_csharp.httpclient
{
    public class DefaultHTTPClient : IHTTPClient
    {
        public HTTPResponse fetch(HTTPRequest request)
        {
            UriBuilder uriBuilder = new UriBuilder(request.url);
            StringBuilder query = new StringBuilder();
            foreach (var parameter in request.parameters)
            {
                query.Append(parameter.Key);
                query.Append("=");
                query.Append(parameter.Value);
                query.Append("&");
            }
            query.Length--;  // Remove the last '&' character
            uriBuilder.Query = query.ToString();

            HttpRequestMessage msg = new HttpRequestMessage();
            msg.RequestUri = uriBuilder.Uri;
            request.url = uriBuilder.Uri.ToString();
            msg.Method = new HttpMethod(request.method.ToString());
            msg.Content = new StringContent(request.body, Encoding.UTF8);
            foreach (var header in request.headers)
            {
                msg.Headers.Add(header.Key, header.Value);
            }

            // Create http client
            using (var client = new HttpClient())
            {
                // Fetch http response
                try
                {
                    // Sync send request
                    HttpResponseMessage response = client.SendAsync(msg).Result;
                    // Sync read response body
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    HTTPResponse res = new HTTPResponse()
                        .setRequest(request)
                        .setStatusCode((int)response.StatusCode)
                        .setReason(response.ReasonPhrase)
                        .setBody(responseBody);

                    foreach (var header in response.Headers)
                    {
                        foreach (var value in header.Value)
                        {
                            res.addHeader(header.Key, value);
                        }
                    }

                    return res;
                }
                catch (HttpRequestException)
                {
                    // not handle, re-throw
                    throw;
                }
            }
        }

        public void close()
        { }
    }
}