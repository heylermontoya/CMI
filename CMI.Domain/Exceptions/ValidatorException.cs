namespace CMI.Domain.Exceptions
{
    [Serializable]
    public class ValidatorException : Exception
    {
        public ValidatorException() { }

        public ValidatorException(
            string message
        ) : base(message) { }

        public ValidatorException(
            string message,
            Exception inner
        ) : base(message, inner) { }

        protected ValidatorException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context
        ) : base(info, context) { }
    }
}
