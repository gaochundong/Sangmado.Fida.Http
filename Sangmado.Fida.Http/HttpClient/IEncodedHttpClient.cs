using System.Net;

namespace Sangmado.Fida.Http
{
    public interface IEncodedHttpClient
    {
        T Get<T>(string url);
        T Get<T>(string url, out HttpStatusCode statusCode);

        HttpStatusCode Put(string url, object content);
        T Put<T>(string url, object content);
        T Put<T>(string url, object content, out HttpStatusCode statusCode);

        HttpStatusCode PutEncoded(string url, byte[] content);
        T PutEncoded<T>(string url, byte[] content);

        HttpStatusCode Post(string url, object content);
        T Post<T>(string url, object content);
        T Post<T>(string url, object content, out HttpStatusCode statusCode);

        HttpStatusCode PostEncoded(string url, byte[] content);
        T PostEncoded<T>(string url, byte[] content);
        T PostEncoded<T>(string url, byte[] content, out HttpStatusCode statusCode);

        HttpStatusCode Delete(string url);
        T Delete<T>(string url);
        T Delete<T>(string url, out HttpStatusCode statusCode);
    }
}
