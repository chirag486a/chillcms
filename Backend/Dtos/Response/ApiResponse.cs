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
        public dynamic? Data { get; private set; }
        public int? Total { get; private set; }
        public string Message { get; private set; }
        public bool Status { get; private set; }
        public Dictionary<string, List<string>>? Errors { get; private set; }

        public ApiResponse()
        {
            Errors = new Dictionary<string, List<string>>();
            Status = false;
            Message = "Hello World!";
        }

        public static ApiResponse<T> Success(T data, string message = "Operation successful")
        {
            return new ApiResponse<T>
            {
                Data = data,
                Status = true,
                Message = message,
                Errors = null,
                Total = null
            };
        }
        public static ApiResponse<T> Success(List<T> data, int total, string message = "Operation successfull")
        {
            return new ApiResponse<T>
            {
                Data = data,
                Total = total,
                Status = true,
                Message = message,
                Errors = null
            };
        }
        public static ApiResponse<T> Error(string Message = "Operation failed!", T? data = default)
        {
            return new ApiResponse<T>
            {
                Data = data,
                Status = false,
                Message = Message,
            };
        }
        public static ApiResponse<T> Error(string Key, string Message, T? data = default)
        {
            var errors = new Dictionary<string, List<string>>
            {
                { Key, [Message] }
            };
            return new ApiResponse<T>
            {
                Data = data,
                Status = false,
                Message = Message,
                Errors = errors
            };
        }
        public void AddError(string Key, string Message)
        {
            if (Errors == null)
            {
                return;
            }
            if (!Errors.ContainsKey(Key))
            {
                Errors[Key] = [Message];
                return;
            }
            Errors[Key].Add(Message);
        }
    }
}