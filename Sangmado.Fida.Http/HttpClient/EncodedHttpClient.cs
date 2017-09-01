using System;
using System.Net;
using System.Net.Http;
using Sangmado.Fida.MessageEncoding;
using Sangmado.Inka.Logging;

namespace Sangmado.Fida.Http
{
    public class EncodedHttpClient : IEncodedHttpClient
    {
        private static ILog _log = Logger.Get<EncodedHttpClient>();
        private static HttpClient _httpClient;
        private IMessageEncoder _encoder;
        private IMessageDecoder _decoder;

        static EncodedHttpClient()
        {
            var defaultHttpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(60) };
            SetHttpClient(defaultHttpClient);
        }

        public static void SetHttpClient(HttpClient httpClient)
        {
            if (httpClient != null)
            {
                _httpClient = httpClient;
            }
        }

        public EncodedHttpClient(IMessageEncoder encoder, IMessageDecoder decoder)
        {
            if (encoder == null)
                throw new ArgumentNullException("encoder");
            if (decoder == null)
                throw new ArgumentNullException("decoder");

            _encoder = encoder;
            _decoder = decoder;
        }

        #region GET

        public HttpStatusCode Get(string url)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            try
            {
                var response = _httpClient.GetAsync(url).Result;
                statusCode = response.StatusCode;
                if (!response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    _log.WarnFormat("Get, Url[{0}], StatusCode[{1}|{2}].",
                        url, (int)response.StatusCode, response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Get, Url[{0}], Error[{1}].", url, ex.Message), ex);
                throw new HttpRequestException(ex.Message, ex);
            }

            return statusCode;
        }

        public T Get<T>(string url)
        {
            HttpStatusCode throwAway;
            return Get<T>(url, out throwAway);
        }

        public T Get<T>(string url, out HttpStatusCode statusCode)
        {
            T result = default(T);
            statusCode = HttpStatusCode.InternalServerError;

            try
            {
                byte[] responseBody = null;
                var response = _httpClient.GetAsync(url).Result;
                statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    _log.WarnFormat("Get, Url[{0}], StatusCode[{1}|{2}].",
                        url, (int)response.StatusCode, response.StatusCode.ToString());

                    throw new UnanticipatedResponseException(
                        string.Format("HTTP [{0}] request to Uri[{1}] response with StatusCode[{2}|{3}] was unanticipated.",
                            response.RequestMessage.Method, response.RequestMessage.RequestUri, (int)response.StatusCode, response.StatusCode.ToString()));
                }

                if (responseBody != null && responseBody.Length > 0)
                {
                    result = _decoder.DecodeMessage<T>(responseBody, 0, responseBody.Length);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Get, Url[{0}], Error[{1}].", url, ex.Message), ex);
                throw new HttpRequestException(ex.Message, ex);
            }

            return result;
        }

        #endregion

        #region PUT

        public HttpStatusCode Put(string url, object content)
        {
            return PutEncoded(url, _encoder.EncodeMessage(content));
        }

        public T Put<T>(string url, object content)
        {
            return PutEncoded<T>(url, _encoder.EncodeMessage(content));
        }

        public T Put<T>(string url, object content, out HttpStatusCode statusCode)
        {
            return PutEncoded<T>(url, _encoder.EncodeMessage(content), out statusCode);
        }

        public HttpStatusCode PutEncoded(string url, byte[] content)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            try
            {
                var httpContent = new ByteArrayContent(content);
                var response = _httpClient.PutAsync(url, httpContent).Result;
                statusCode = response.StatusCode;
                if (!response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    _log.WarnFormat("Put, Url[{0}], StatusCode[{1}|{2}].",
                        url, (int)response.StatusCode, response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Put, Url[{0}], Error[{1}].", url, ex.Message), ex);
                throw new HttpRequestException(ex.Message, ex);
            }

            return statusCode;
        }

        public T PutEncoded<T>(string url, byte[] content)
        {
            HttpStatusCode throwAway;
            return PutEncoded<T>(url, content, out throwAway);
        }

        public T PutEncoded<T>(string url, byte[] content, out HttpStatusCode statusCode)
        {
            T result = default(T);
            statusCode = HttpStatusCode.InternalServerError;

            try
            {
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                var response = _httpClient.PutAsync(url, httpContent).Result;
                statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    _log.WarnFormat("Put, Url[{0}], StatusCode[{1}|{2}].",
                        url, (int)response.StatusCode, response.StatusCode.ToString());

                    throw new UnanticipatedResponseException(
                        string.Format("HTTP [{0}] request to Uri[{1}] response with StatusCode[{2}|{3}] was unanticipated.",
                            response.RequestMessage.Method, response.RequestMessage.RequestUri, (int)response.StatusCode, response.StatusCode.ToString()));
                }

                if (responseBody != null && responseBody.Length > 0)
                {
                    result = _decoder.DecodeMessage<T>(responseBody, 0, responseBody.Length);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Put, Url[{0}], Error[{1}].", url, ex.Message), ex);
                throw new HttpRequestException(ex.Message, ex);
            }

            return result;
        }

        #endregion

        #region POST

        public HttpStatusCode Post(string url, object content)
        {
            return PostEncoded(url, _encoder.EncodeMessage(content));
        }

        public T Post<T>(string url, object content)
        {
            return PostEncoded<T>(url, _encoder.EncodeMessage(content));
        }

        public T Post<T>(string url, object content, out HttpStatusCode statusCode)
        {
            return PostEncoded<T>(url, _encoder.EncodeMessage(content), out statusCode);
        }

        public HttpStatusCode PostEncoded(string url, byte[] content)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            try
            {
                var httpContent = new ByteArrayContent(content);
                var response = _httpClient.PostAsync(url, httpContent).Result;
                statusCode = response.StatusCode;
                if (!response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    _log.WarnFormat("Post, Url[{0}], StatusCode[{1}|{2}].",
                        url, (int)response.StatusCode, response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Post, Url[{0}], Error[{1}].", url, ex.Message), ex);
                throw new HttpRequestException(ex.Message, ex);
            }

            return statusCode;
        }

        public T PostEncoded<T>(string url, byte[] content)
        {
            HttpStatusCode throwAway;
            return PostEncoded<T>(url, content, out throwAway);
        }

        public T PostEncoded<T>(string url, byte[] content, out HttpStatusCode statusCode)
        {
            T result = default(T);
            statusCode = HttpStatusCode.InternalServerError;

            try
            {
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                var response = _httpClient.PostAsync(url, httpContent).Result;
                statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    _log.WarnFormat("Post, Url[{0}], StatusCode[{1}|{2}].",
                        url, (int)response.StatusCode, response.StatusCode.ToString());

                    throw new UnanticipatedResponseException(
                        string.Format("HTTP [{0}] request to Uri[{1}] response with StatusCode[{2}|{3}] was unanticipated.",
                            response.RequestMessage.Method, response.RequestMessage.RequestUri, (int)response.StatusCode, response.StatusCode.ToString()));
                }

                if (responseBody != null && responseBody.Length > 0)
                {
                    result = _decoder.DecodeMessage<T>(responseBody, 0, responseBody.Length);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Post, Url[{0}], Error[{1}].", url, ex.Message), ex);
                throw new HttpRequestException(ex.Message, ex);
            }

            return result;
        }

        #endregion

        #region DELETE

        public HttpStatusCode Delete(string url)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            try
            {
                var response = _httpClient.DeleteAsync(url).Result;
                statusCode = response.StatusCode;
                if (!response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    _log.WarnFormat("Delete, Url[{0}], StatusCode[{1}|{2}].",
                        url, (int)response.StatusCode, response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Delete, Url[{0}], Error[{1}].", url, ex.Message), ex);
                throw new HttpRequestException(ex.Message, ex);
            }

            return statusCode;
        }

        public T Delete<T>(string url)
        {
            HttpStatusCode throwAway;
            return Delete<T>(url, out throwAway);
        }

        public T Delete<T>(string url, out HttpStatusCode statusCode)
        {
            T result = default(T);
            statusCode = HttpStatusCode.InternalServerError;

            try
            {
                byte[] responseBody = null;
                var response = _httpClient.DeleteAsync(url).Result;
                statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    _log.WarnFormat("Delete, Url[{0}], StatusCode[{1}|{2}].",
                        url, (int)response.StatusCode, response.StatusCode.ToString());

                    throw new UnanticipatedResponseException(
                        string.Format("HTTP [{0}] request to Uri[{1}] response with StatusCode[{2}|{3}] was unanticipated.",
                            response.RequestMessage.Method, response.RequestMessage.RequestUri, (int)response.StatusCode, response.StatusCode.ToString()));
                }

                if (responseBody != null && responseBody.Length > 0)
                {
                    result = _decoder.DecodeMessage<T>(responseBody, 0, responseBody.Length);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Delete, Url[{0}], Error[{1}].", url, ex.Message), ex);
                throw new HttpRequestException(ex.Message, ex);
            }

            return result;
        }

        #endregion
    }
}
