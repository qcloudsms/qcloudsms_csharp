namespace qcloudsms_csharp.httpclient
{

    public interface IHTTPClient
    {
        HTTPResponse fetch(HTTPRequest request);

        void close();
    }
}