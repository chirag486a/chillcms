using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Interfaces.IServices;
using Backend.Models.Contents;
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
            try
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
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw new Exception(err.Message);
            }
        }
        public async Task CreateContentDirectory(string UserId, ContentMeta contentMeta)
        {
            try
            {
                if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(contentMeta.Id))
                {
                    throw new ArgumentException("User id or Content Id Cannot be null");
                }
                string userFolderPath = Path.Combine(_baseDirectory, UserId);
                string contentFolderPath = Path.Combine(_baseDirectory, UserId, contentMeta.Id);
                if (!Directory.Exists(userFolderPath))
                {
                    await Task.Run(() => Directory.CreateDirectory(userFolderPath));
                }

                if (!Directory.Exists(contentFolderPath))
                {
                    Console.WriteLine("hello");
                    await Task.Run(() => Directory.CreateDirectory(contentFolderPath));
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw new Exception(err.Message);
            }
        }
    }
}