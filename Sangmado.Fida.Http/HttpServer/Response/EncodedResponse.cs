using System;
using System.IO;
using System.Net;
using Happer.Http;
using Happer.Serialization;
using Sangmado.Fida.Messaging;
using Sangmado.Inka.Logging;

namespace Sangmado.Fida.Http
{
    public class EncodedResponse<TModel> : Response
    {
        private static ILog _log = Logger.Get<EncodedResponse>();

        public EncodedResponse(TModel model, IMessageEncoder encoder)
        {
            if (encoder == null)
                throw new ArgumentNullException("encoder");

            this.ContentType = @"application/octet-stream";
            this.Contents = model == null ? NoBody : GetEncodedContents(model, encoder);
            this.StatusCode = HttpStatusCode.OK;
        }

        private static Action<Stream> GetEncodedContents(TModel model, IMessageEncoder encoder)
        {
            return outputStream =>
            {
                try
                {
                    using (var stream = new UnclosableStreamWrapper(outputStream))
                    {
                        var buffer = encoder.EncodeMessage(model);
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message, ex);
                }
            };
        }
    }

    public class EncodedResponse : EncodedResponse<object>
    {
        public EncodedResponse(object model, IMessageEncoder encoder)
            : base(model, encoder)
        {
        }
    }
}
