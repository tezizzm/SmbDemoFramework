using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LighthouseWeb.Models;

namespace LighthouseWeb
{
    public interface ISmbClient : IDisposable
    {
        Task<FileDetails> ReadAsync(string file);
        Task WriteAsync(string file, string data);
        IEnumerable<FileInfo> GetAllFiles();
        void DeleteFile(string file);
        List<PermissionViewModel> DirectoryInformation();
    }
}