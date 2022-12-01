using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FakeDbTestModel.Exceptions
{
    public class UpdateNotAllowedException : Exception
    {
        public UpdateNotAllowedException()
        {
        }

        public UpdateNotAllowedException(string? message) : base(message)
        {
        }

        public UpdateNotAllowedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UpdateNotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
