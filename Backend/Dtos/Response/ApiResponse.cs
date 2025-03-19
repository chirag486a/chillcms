using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend.Dtos.Response
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public ApiResponse()
        {
            Errors = new Dictionary<string, string>();
            Status = false;
            Message = "Hello World!";
        }

        public static ApiResponse<T> Success(T data, string message = "Operation successful")
        {
            return new ApiResponse<T>
            {
                Data = data,
                Status = true,
                Message = message
            };
        }

        public static ApiResponse<T> Error(string Key, string Message, T? data = default)
        {
            var errors = new Dictionary<string, string>();
            errors.Add(Key, Message);
            return new ApiResponse<T>
            {
                Data = data,
                Status = false,
                Message = Message,
                Errors = errors
            };

        }
    }
}