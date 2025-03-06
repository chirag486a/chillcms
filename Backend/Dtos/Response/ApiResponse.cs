using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos.Response
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public List<string> Errors;

        public ApiResponse()
        {
            Errors = new List<string>();
            Status = false;
            Message = "Hello World!";
        }

        public static ApiResponse<T> Success(T data, string message = "Operation successfull")
        {
            return new ApiResponse<T>
            {
                Data = data,
                Status = true,
                Message = message
            };
        }
        public static ApiResponse<T> Error(string errorMessage, T? data = default)
        {
            return new ApiResponse<T>
            {
                Data = data,
                Status = true,
                Message = "Operation failed",
                Errors = new List<string> { errorMessage }
            };
        }
        public static ApiResponse<T> Error(List<string> errors, T? data = default)
        {
            return new ApiResponse<T>
            {
                Data = data,
                Status = true,
                Message = "Operation failed",
                Errors = errors
            };
        }
    }
}