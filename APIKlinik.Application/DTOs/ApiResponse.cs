using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.DTOs
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(int code, string message, T data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        // Factory methods for common responses
        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T>(200, "Success", data);
        }

        public static ApiResponse<T> Success(string message, T data)
        {
            return new ApiResponse<T>(200, message, data);
        }

        public static ApiResponse<T> NotFound(string message = "Resource not found")
        {
            return new ApiResponse<T>(404, message, default);
        }

        public static ApiResponse<T> BadRequest(string message = "Bad request")
        {
            return new ApiResponse<T>(400, message, default);
        }

        public static ApiResponse<T> Unauthorized(string message = "Unauthorized")
        {
            return new ApiResponse<T>(401, message, default);
        }

        public static ApiResponse<T> InternalError(string message = "Internal server error")
        {
            return new ApiResponse<T>(500, message, default);
        }
    }
}
