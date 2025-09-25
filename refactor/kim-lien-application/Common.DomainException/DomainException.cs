using System.Net;

namespace Common.DomainException
{
    public interface IResultException
    {
        object Result { get; }
    }
    public class DomainException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public int? ErrorCode { get; set; }

        public DomainException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, int? errorCode = null) : base(message)
        {
            HttpStatusCode = httpStatusCode;
            ErrorCode = errorCode;
        }
    }

    public class DomainException<T> : DomainException, IResultException
    {
        public required T ResponseData { get; set; }
        public object Result => ResponseData!;

        public DomainException(string message, T responseData, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, int? errorCode = null) : base(message, httpStatusCode, errorCode)
        {
            ResponseData = responseData;
        }
    }
}
