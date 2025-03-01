using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models.Users;

namespace Backend.Interfaces.IServices
{
    public interface IFileService
    {
        public Task CreateUserDirectoryAsync(User user);
    }
}