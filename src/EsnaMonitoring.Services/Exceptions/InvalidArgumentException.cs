using System;

namespace EsnaMonitoring.Services.Exceptions
{
    [Serializable]
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException() { }
        public InvalidArgumentException(string argument) : base($"'{argument}' is invalid") { }
        public InvalidArgumentException(string argument, Exception inner) : base($"'{argument}' is invalid", inner) { }
        protected InvalidArgumentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
