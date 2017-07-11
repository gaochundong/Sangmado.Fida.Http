using System;

namespace Sangmado.Fida.Http
{
    [Serializable]
    public class UnanticipatedResponseException : Exception
    {
        public UnanticipatedResponseException()
            : base()
        {
        }

        public UnanticipatedResponseException(string message)
            : base(message)
        {
        }

        public UnanticipatedResponseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
