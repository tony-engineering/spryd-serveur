using System;
using System.Runtime.Serialization;
using Spryd.Serveur.Models;

namespace Spryd.Serveur.Tests
{
    [Serializable]
    public class NotValidUserException : Exception
    {

        public NotValidUserException()
        {
        }

        public NotValidUserException(string message) : base(message)
        {
        }

        public NotValidUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotValidUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}