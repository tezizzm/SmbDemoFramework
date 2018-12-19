using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using LighthouseWeb.Models;

namespace LighthouseWeb
{
    public interface ISmbService : IDisposable
    {
        IEnumerable<FileViewModel> GetFiles();
        Task<FileDetailViewModel> GetFile(string file);
        Task CreateFile(HttpPostedFileBase file);
        void DeleteFile(string path);
        Task<DeleteFileViewModel> GetDeleteModel(string name);
        List<PermissionViewModel> GetFolderPermissions();
    }
}