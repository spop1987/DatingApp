
using System.Net;

namespace API.Helpers
{
    public class Error
    {
        public string Message { get; set; }
        public int StatusCode  { get; set; }
    }

    public static class DatingAppError
    {
        public static Error Error(ErrorActivity activity)
        {
            return new Error
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = $"Error tring to process activity: {(ErrorActivity)activity}"
            };
        }

        public static Error SystemFailure(ErrorActivity activity)
        {
            return new Error
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = $"Internal Server Error trying to process activity: {(ErrorActivity)activity}"
            };
        }

        public static Error CreateDuplicated(ErrorActivity activity)
        {
            return new Error
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = $"Error trying to create a duplicated user. Activity: {(ErrorActivity)activity}"
            };
        }

        public static Error NotFound(ErrorActivity activity)
        {
            return new Error
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = $"Not found User. Activity: {(ErrorActivity)activity}"
            };
        }
    }

    public abstract class DatingAppException : Exception
    {
        public Error Error { get; set; }
        protected DatingAppException(Error error) : base() { Error = error; }
        protected DatingAppException(string message) : base(message) {}
    }

    public class DatingAppInvalidUserException : DatingAppException
    {
        public DatingAppInvalidUserException(Error error) : base(error) {}
        public DatingAppInvalidUserException(string message) : base(message) {}
        public DatingAppInvalidUserException() : base(string.Empty) {}
    }

    public class DatingAppNotFoundUserException : DatingAppException
    {
        public DatingAppNotFoundUserException(Error error) : base(error) {}
        public DatingAppNotFoundUserException(string message) : base(message) {}
        public DatingAppNotFoundUserException() : base(string.Empty) {}
    }

    public class DatingAppNotFoundUsersException : DatingAppException
    {
        public DatingAppNotFoundUsersException(Error error) : base(error) {}
        public DatingAppNotFoundUsersException(string message) : base(message) {}
        public DatingAppNotFoundUsersException() : base(string.Empty) {}
    }
}