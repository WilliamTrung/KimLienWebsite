using System.Net;
using Common.Kernel.Constants;

namespace Common.DomainException.Abstractions
{
    public interface IResultException
    {
        object Result { get; }
    }
    public class CException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public int? ErrorCode { get; set; }

        public CException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, int? errorCode = null) : base(message)
        {
            HttpStatusCode = httpStatusCode;
            ErrorCode = errorCode;
        }
    }

    public class CException<T> : CException, IResultException
    {
        public required T ResponseData { get; set; }
        public object Result => ResponseData!;

        public CException(string message, T responseData, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, int? errorCode = null) : base(message, httpStatusCode, errorCode)
        {
            ResponseData = responseData;
        }
    }
    public class CNotFoundException : CException
    {
        public CNotFoundException(Type notfoundType) : base($"{nameof(notfoundType)} not found!", HttpStatusCode.NotFound, (int)ResponseCode.NotFound)
        {
        }

        public CNotFoundException(string message) : base(message, HttpStatusCode.NotFound, (int)ResponseCode.NotFound)
        {
        }
    }
    public class CInvalidException : CException
    {
        public CInvalidException(Type invalidType) : base($"{nameof(invalidType)} invalid!", HttpStatusCode.BadRequest, (int)ResponseCode.Invalid)
        {
        }

        public CInvalidException(string message) : base(message, HttpStatusCode.BadRequest, (int)ResponseCode.Invalid)
        {
        }
    }
}
