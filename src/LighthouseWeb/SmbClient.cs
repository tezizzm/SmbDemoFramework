using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LighthouseWeb.Models;
using Steeltoe.Common.Net;

namespace LighthouseWeb
{
    public class SmbClient : ISmbClient
    {
        public string UserName { get; }
        public string Password { get; }
        public string Domain { get; }
        public string UncPath { get; }

        private readonly WindowsNetworkFileShare _fileShare;
        private static readonly Regex Regex = new Regex(@"[\S]+", RegexOptions.Compiled & RegexOptions.Multiline);

        public SmbClient(string uncPath, string userName = "", string password = "", string domain = "")
        {
            UncPath = uncPath ?? throw new ArgumentNullException(nameof(uncPath));
            UserName = userName;
            Password = password;
            Domain = domain;

            Console.WriteLine($@"Establishing credentials for user {domain}\{userName}");
            var credential = new NetworkCredential(UserName, Password, Domain);

            Console.WriteLine($"Logging into share at path {uncPath}");
            _fileShare = new WindowsNetworkFileShare(uncPath, credential);
            Console.WriteLine($"Logged into share at path {uncPath}");
        }

        public async Task WriteAsync(string file, string data)
        {
            Console.WriteLine($"Writing data to {file}");
            using (var streamWriter = new StreamWriter($"{UncPath}\\{file}"))
            {
                await streamWriter.WriteLineAsync(data);
            }
        }

        public async Task<FileDetails> ReadAsync(string file)
        {
            var fileAndPath = $"{UncPath}\\{file}";
            var fileInfo = new FileInfo(fileAndPath);
            using (var stream = fileInfo.OpenText())
            {
                var task = stream.ReadToEndAsync();
                Console.WriteLine($"Reading data from {fileAndPath}");
                var fileDetail = new FileDetails
                {
                    Directory = fileInfo.DirectoryName,
                    Length = fileInfo.Length,
                    Name = fileInfo.Name,
                    LineCount = File.ReadLines(fileAndPath).Count()
                };

                var content = await task;
                fileDetail.FileContent = content;

                fileDetail.WordCount = Regex.Matches(content).Count;
                return fileDetail;
            }
        }

        public IEnumerable<FileInfo> GetAllFiles()
        {
            Console.WriteLine("Reading all the files in the shared directory");
            return new DirectoryInfo(UncPath).GetFiles();
        }

        public void DeleteFile(string file)
        {
            File.Delete($"{UncPath}\\{file}");
        }

        public List<PermissionViewModel> DirectoryInformation()
        {
            var di = new DirectoryInfo(UncPath);
            var acl = di.GetAccessControl();
            var rules = acl.GetAccessRules(true, true, typeof(NTAccount));

            var result = new List<PermissionViewModel>();
            Console.WriteLine($"User in the Directory Info method {UserName}");
            foreach (FileSystemAccessRule authorizationRule in rules)
            {
                result.Add(new PermissionViewModel
                {
                    Access = authorizationRule.AccessControlType == AccessControlType.Deny
                        ? Access.Denied
                        : Access.Allowed,
                    Directory = UncPath,
                    User = authorizationRule.IdentityReference.Value,
                    Rights = authorizationRule.FileSystemRights.ToString(),
                    LoggedInUser = UserName
                });
            }

            return result;
        }

        public void Dispose()
        {
            _fileShare?.Dispose();
        }
    }
}