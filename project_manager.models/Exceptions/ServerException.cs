using System;

namespace project_manager.common.Exceptions
{
    public class ServerException: Exception
    {
        public int StatusCode { get; private set; }
        public ServerException(int statusCode, string errorMessage): base(errorMessage)
        {
            StatusCode = statusCode;
        }
    }
}
