using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models.Contents;
using Backend.Models.Users;

namespace Backend.Interfaces.IServices
{
    public interface IFileService
    {
        public Task CreateUserDirectoryAsync(User user);
        public Task CreateContentDirectory(string userId, ContentMeta contentMeta);

    }
}