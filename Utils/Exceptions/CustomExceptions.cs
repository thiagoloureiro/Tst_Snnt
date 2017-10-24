using System;

namespace Utils.Exceptions
{
    public class CustomExceptions : Exception
    {
        public string Code { get; }

        public CustomExceptions()
        {
        }

        public CustomExceptions(string code)
        {
            Code = code;
        }

        public CustomExceptions(string code, string message)
            : base(message)
        {
            Code = code;
        }

        public CustomExceptions(string code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }
    }
}