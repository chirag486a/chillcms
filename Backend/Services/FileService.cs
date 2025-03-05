using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Backend.Interfaces.IServices;
using Backend.Models.Contents;
using Backend.Models.Users;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

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
                    await Task.Run(() => Directory.CreateDirectory(contentFolderPath));
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw new Exception(err.Message);
            }
        }

        public async Task CreateFormatDirectory(string userId, string contentId, string format)
        {
            try
            {
                var FormatPath = Path.Combine(_baseDirectory, userId, contentId, format);
                if (!Directory.Exists(FormatPath))
                {
                    await Task.Run(() => Directory.CreateDirectory(FormatPath));
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw new Exception(err.Message);
            }
        }
        public async Task SaveContent(string UserId, Content c, IFormFile file)
        {
            try
            {
                var saveDirectory = Path.Combine(_baseDirectory, UserId, c.ContentMetaId, c.Format);
                var extension = Path.GetExtension(file.FileName);
                var filePath = Path.Combine(saveDirectory, c.Id + extension);

                using (var stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (UnauthorizedAccessException err)
            {
                Console.WriteLine(err);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }


        }
        public FileStream GetContent(ContentMeta cm, Content c)
        {
            var filePath = Path.Combine(_baseDirectory, cm.UserId, c.ContentMetaId, c.Format, c.Id + Path.GetExtension(c.FileName));
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }
    }
}