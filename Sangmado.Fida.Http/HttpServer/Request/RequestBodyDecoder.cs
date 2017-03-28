using System.Net;
using Happer.Http;
using Sangmado.Fida.Messaging;

namespace Sangmado.Fida.Http
{
    public static class RequestBodyDecoder
    {
        public static HttpStatusCode Decode<T>(Request request, IBufferManager bufferManager, IMessageDecoder decoder, out T result)
        {
            result = default(T);

            var buffer = bufferManager.BorrowBuffer();
            int count = request.Body.Read(buffer, 0, buffer.Length);
            if (count == 0 || count == buffer.Length)
            {
                bufferManager.ReturnBuffer(buffer);
                return HttpStatusCode.RequestEntityTooLarge;
            }

            result = decoder.DecodeMessage<T>(buffer, 0, count);
            bufferManager.ReturnBuffer(buffer);
            if (result == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.OK;
        }
    }
}
