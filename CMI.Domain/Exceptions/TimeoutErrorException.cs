namespace CMI.Domain.Exceptions
{
    [Serializable]
    public class TimeoutErrorException : Exception
    {
        public TimeoutErrorException() { }

        public TimeoutErrorException(
            string message
        ) : base(message) { }

        public TimeoutErrorException(
            string message,
            Exception inner
        ) : base(message, inner) { }

        protected TimeoutErrorException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context
        ) : base(info, context) { }
    }
}
