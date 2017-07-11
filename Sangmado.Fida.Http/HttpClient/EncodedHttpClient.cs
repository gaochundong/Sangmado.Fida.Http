using System;
using System.Net;
using System.Net.Http;
using Sangmado.Fida.Messaging;
using Sangmado.Inka.Logging;

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
            _httpClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
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
            T result = default(T);

            try
            {
                byte[] responseBody = null;
                var response = _httpClient.GetAsync(url).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                }
                else
                {
                    _log.WarnFormat("Get, Url[{0}], StatusCode[{1}|{2}].",
                        url, response.StatusCode, response.StatusCode.ToString());

                    // NotFound will return a null object
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

        public void Put(string url, object content)
        {
            PutEncoded(url, _encoder.EncodeMessage(content));
        }

        public T Put<T>(string url, object content)
        {
            return PutEncoded<T>(url, _encoder.EncodeMessage(content));
        }

        public void PutEncoded(string url, byte[] content)
        {
            try
            {
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                var response = _httpClient.PutAsync(url, httpContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                }
                else
                {
                    _log.WarnFormat("Put, Url[{0}], StatusCode[{1}|{2}].",
                        url, response.StatusCode, response.StatusCode.ToString());

                    // NotFound will do nothing
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
        }

        public T PutEncoded<T>(string url, byte[] content)
        {
            T result = default(T);

            try
            {
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                var response = _httpClient.PutAsync(url, httpContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                }
                else
                {
                    _log.WarnFormat("Put, Url[{0}], StatusCode[{1}|{2}].",
                        url, response.StatusCode, response.StatusCode.ToString());

                    // NotFound will return a null object
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

        public void Post(string url, object content)
        {
            PostEncoded(url, _encoder.EncodeMessage(content));
        }

        public T Post<T>(string url, object content)
        {
            return PostEncoded<T>(url, _encoder.EncodeMessage(content));
        }

        public void PostEncoded(string url, byte[] content)
        {
            try
            {
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                var response = _httpClient.PostAsync(url, httpContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
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
        }

        public T PostEncoded<T>(string url, byte[] content)
        {
            T result = default(T);

            try
            {
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                var response = _httpClient.PostAsync(url, httpContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
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
                result = default(T);
            }

            return result;
        }

        #endregion
    }
}
