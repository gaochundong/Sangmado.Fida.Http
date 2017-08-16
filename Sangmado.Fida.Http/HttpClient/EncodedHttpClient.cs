using System;
using System.Net;
using System.Net.Http;
using Sangmado.Inka.Logging;
using Sangmado.Inka.MessageEncoding;

namespace Sangmado.Fida.Http
{
    public class EncodedHttpClient : IEncodedHttpClient
    {
        private static ILog _log = Logger.Get<EncodedHttpClient>();

        private IMessageEncoder _encoder;
        private IMessageDecoder _decoder;

        private static readonly HttpClient _httpClient;

        static EncodedHttpClient()
        {
            _httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(60) };
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

        public T Get<T>(string url)
        {
            return Get<T>(url, out HttpStatusCode throwAway);
        }

        public T Get<T>(string url, out HttpStatusCode statusCode)
        {
            T result = default(T);
            statusCode = HttpStatusCode.InternalServerError;

            try
            {
                byte[] responseBody = null;
                var response = _httpClient.GetAsync(url).GetAwaiter().GetResult();
                statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                }
                else
                {
                    _log.WarnFormat("Get, Url[{0}], StatusCode[{1}|{2}].",
                        url, response.StatusCode, response.StatusCode.ToString());

                    // NotFound indicates that the requested resource does not exist on the server.
                    if (response.StatusCode != HttpStatusCode.NotFound)
                    {
                        // otherwise, any other status code within response means unsuccessful
                        throw new UnanticipatedResponseException(
                            string.Format("HTTP [GET] response with StatusCode[{0}|{1}] was unanticipated.",
                                response.StatusCode, response.StatusCode.ToString()));
                    }
                }

                if (responseBody != null && responseBody.Length > 0)
                {
                    result = _decoder.DecodeMessage<T>(responseBody, 0, responseBody.Length);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Get, Url[{0}], Error[{1}].", url, ex.Message), ex);
                result = default(T);
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
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                var response = _httpClient.PutAsync(url, httpContent).GetAwaiter().GetResult();
                statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                }
                else
                {
                    _log.WarnFormat("Put, Url[{0}], StatusCode[{1}|{2}].",
                        url, response.StatusCode, response.StatusCode.ToString());

                    // NotFound indicates that the requested resource does not exist on the server.
                    if (response.StatusCode != HttpStatusCode.NotFound)
                    {
                        // otherwise, any other status code within response means unsuccessful
                        throw new UnanticipatedResponseException(
                            string.Format("HTTP [PUT] response with StatusCode[{0}|{1}] was unanticipated.",
                                response.StatusCode, response.StatusCode.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Put, Url[{0}], Error[{1}].", url, ex.Message), ex);
            }

            return statusCode;
        }

        public T PutEncoded<T>(string url, byte[] content)
        {
            return PutEncoded<T>(url, content, out HttpStatusCode throwAway);
        }

        public T PutEncoded<T>(string url, byte[] content, out HttpStatusCode statusCode)
        {
            T result = default(T);
            statusCode = HttpStatusCode.InternalServerError;

            try
            {
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                var response = _httpClient.PutAsync(url, httpContent).GetAwaiter().GetResult();
                statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                }
                else
                {
                    _log.WarnFormat("Put, Url[{0}], StatusCode[{1}|{2}].",
                        url, response.StatusCode, response.StatusCode.ToString());

                    // NotFound indicates that the requested resource does not exist on the server.
                    if (response.StatusCode != HttpStatusCode.NotFound)
                    {
                        // otherwise, any other status code within response means unsuccessful
                        throw new UnanticipatedResponseException(
                            string.Format("HTTP [PUT] response with StatusCode[{0}|{1}] was unanticipated.",
                                response.StatusCode, response.StatusCode.ToString()));
                    }
                }

                if (responseBody != null && responseBody.Length > 0)
                {
                    result = _decoder.DecodeMessage<T>(responseBody, 0, responseBody.Length);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Put, Url[{0}], Error[{1}].", url, ex.Message), ex);
                result = default(T);
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
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                var response = _httpClient.PostAsync(url, httpContent).GetAwaiter().GetResult();
                statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                }
                else
                {
                    // any other status code within response means unsuccessful
                    throw new UnanticipatedResponseException(
                        string.Format("HTTP [POST] response with StatusCode[{0}|{1}] was unanticipated.",
                            response.StatusCode, response.StatusCode.ToString()));
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Post, Url[{0}], Error[{1}].", url, ex.Message), ex);
            }

            return statusCode;
        }

        public T PostEncoded<T>(string url, byte[] content)
        {
            return PostEncoded<T>(url, content, out HttpStatusCode throwAway);
        }

        public T PostEncoded<T>(string url, byte[] content, out HttpStatusCode statusCode)
        {
            T result = default(T);
            statusCode = HttpStatusCode.InternalServerError;

            try
            {
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                var response = _httpClient.PostAsync(url, httpContent).GetAwaiter().GetResult();
                statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                }
                else
                {
                    // any other status code within response means unsuccessful
                    throw new UnanticipatedResponseException(
                        string.Format("HTTP [POST] response with StatusCode[{0}|{1}] was unanticipated.",
                            response.StatusCode, response.StatusCode.ToString()));
                }

                if (responseBody != null && responseBody.Length > 0)
                {
                    result = _decoder.DecodeMessage<T>(responseBody, 0, responseBody.Length);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Post, Url[{0}], Error[{1}].", url, ex.Message), ex);
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
                var response = _httpClient.DeleteAsync(url).GetAwaiter().GetResult();
                statusCode = response.StatusCode;
                if (!response.IsSuccessStatusCode)
                {
                    _log.WarnFormat("Delete, Url[{0}], StatusCode[{1}|{2}].",
                        url, response.StatusCode, response.StatusCode.ToString());

                    // NotFound indicates that the requested resource does not exist on the server.
                    if (response.StatusCode != HttpStatusCode.NotFound)
                    {
                        // otherwise, any other status code within response means unsuccessful
                        throw new UnanticipatedResponseException(
                            string.Format("HTTP [DELETE] response with StatusCode[{0}|{1}] was unanticipated.",
                                response.StatusCode, response.StatusCode.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Delete, Url[{0}], Error[{1}].", url, ex.Message), ex);
            }

            return statusCode;
        }

        public T Delete<T>(string url)
        {
            return Delete<T>(url, out HttpStatusCode throwAway);
        }

        public T Delete<T>(string url, out HttpStatusCode statusCode)
        {
            T result = default(T);
            statusCode = HttpStatusCode.InternalServerError;

            try
            {
                byte[] responseBody = null;
                var response = _httpClient.DeleteAsync(url).GetAwaiter().GetResult();
                statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode) // StatusCode was in the range 200-299;
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                }
                else
                {
                    _log.WarnFormat("Delete, Url[{0}], StatusCode[{1}|{2}].",
                        url, response.StatusCode, response.StatusCode.ToString());

                    // NotFound indicates that the requested resource does not exist on the server.
                    if (response.StatusCode != HttpStatusCode.NotFound)
                    {
                        // otherwise, any other status code within response means unsuccessful
                        throw new UnanticipatedResponseException(
                            string.Format("HTTP [DELETE] response with StatusCode[{0}|{1}] was unanticipated.",
                                response.StatusCode, response.StatusCode.ToString()));
                    }
                }

                if (responseBody != null && responseBody.Length > 0)
                {
                    result = _decoder.DecodeMessage<T>(responseBody, 0, responseBody.Length);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Delete, Url[{0}], Error[{1}].", url, ex.Message), ex);
                result = default(T);
            }

            return result;
        }

        #endregion
    }
}
