using System;
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

        public EncodedHttpClient(IMessageEncoder encoder, IMessageDecoder decoder)
        {
            if (encoder == null)
                throw new ArgumentNullException("encoder");
            if (decoder == null)
                throw new ArgumentNullException("decoder");

            _encoder = encoder;
            _decoder = decoder;
        }

        public T Get<T>(string requestUri)
        {
            T result = default(T);

            try
            {
                byte[] responseBody = null;
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(requestUri).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                    }
                }

                if (responseBody != null && responseBody.Length > 0)
                {
                    result = _decoder.DecodeMessage<T>(responseBody, 0, responseBody.Length);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Get, RequestUri[{0}], Error[{1}].", requestUri, ex.Message), ex);
                result = default(T);
            }

            return result;
        }

        public void Put(string requestUri, object content)
        {
            PutEncoded(requestUri, _encoder.EncodeMessage(content));
        }

        public T Put<T>(string requestUri, object content)
        {
            return PutEncoded<T>(requestUri, _encoder.EncodeMessage(content));
        }

        public void PutEncoded(string requestUri, byte[] content)
        {
            try
            {
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                using (var client = new HttpClient())
                {
                    var response = client.PutAsync(requestUri, httpContent).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Put, RequestUri[{0}], Error[{1}].", requestUri, ex.Message), ex);
            }
        }

        public T PutEncoded<T>(string requestUri, byte[] content)
        {
            T result = default(T);

            try
            {
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                using (var client = new HttpClient())
                {
                    var response = client.PutAsync(requestUri, httpContent).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                    }
                }

                if (responseBody != null && responseBody.Length > 0)
                {
                    result = _decoder.DecodeMessage<T>(responseBody, 0, responseBody.Length);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Put, RequestUri[{0}], Error[{1}].", requestUri, ex.Message), ex);
                result = default(T);
            }

            return result;
        }

        public void Post(string requestUri, object content)
        {
            PostEncoded(requestUri, _encoder.EncodeMessage(content));
        }

        public T Post<T>(string requestUri, object content)
        {
            return PostEncoded<T>(requestUri, _encoder.EncodeMessage(content));
        }

        public void PostEncoded(string requestUri, byte[] content)
        {
            try
            {
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                using (var client = new HttpClient())
                {
                    var response = client.PostAsync(requestUri, httpContent).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Post, RequestUri[{0}], Error[{1}].", requestUri, ex.Message), ex);
            }
        }

        public T PostEncoded<T>(string requestUri, byte[] content)
        {
            T result = default(T);

            try
            {
                byte[] responseBody = null;
                var httpContent = new ByteArrayContent(content);
                using (var client = new HttpClient())
                {
                    var response = client.PostAsync(requestUri, httpContent).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                    }
                }

                if (responseBody != null && responseBody.Length > 0)
                {
                    result = _decoder.DecodeMessage<T>(responseBody, 0, responseBody.Length);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Post, RequestUri[{0}], Error[{1}].", requestUri, ex.Message), ex);
                result = default(T);
            }

            return result;
        }
    }
}
