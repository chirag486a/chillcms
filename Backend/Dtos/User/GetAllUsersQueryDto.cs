using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace Backend.Dtos.User
{
    public class GetAllUsersQueryDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public string SortBy { get; set; } = string.Empty;
        public bool IsDescending { get; set; } = false;
        public string Fields { get; set; } = string.Empty;
        public string ExcludeFields { get; set; } = string.Empty;
    }
}