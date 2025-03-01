using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Interfaces.IServices;
using Backend.Models.Users;
using Microsoft.Extensions.Options;

namespace Backend.Services
{
    public class FileService : IFileService
    {
        private IConfiguration _configuration;
        private string _baseDirectory;
        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
            var temp = configuration.GetSection("FileStorageSettings")["Location"];
            if (string.IsNullOrWhiteSpace(temp))
            {
                _baseDirectory = "D:\\temp\\";
            }
            else _baseDirectory = temp;
        }
        public async Task CreateUserDirectoryAsync(User user)
        {
            if (string.IsNullOrEmpty(user.Id))
            {
                throw new ArgumentException("User id cannot be null or empty");
            }
            string userFolderPath = Path.Combine(_baseDirectory, user.Id);

            if (!Directory.Exists(userFolderPath))
            {
                await Task.Run(() => Directory.CreateDirectory(userFolderPath));
            }

            throw new Exception("User directory could not be created");
        }
    }
}