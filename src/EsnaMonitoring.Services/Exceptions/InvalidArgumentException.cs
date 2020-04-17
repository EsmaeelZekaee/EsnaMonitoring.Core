namespace EsnaMonitoring.Services.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException()
        {
        }

        public InvalidArgumentException(string argument)
            : base($"'{argument}' is invalid")
        {
        }

        public InvalidArgumentException(string argument, Exception inner)
            : base($"'{argument}' is invalid", inner)
        {
        }

        protected InvalidArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}