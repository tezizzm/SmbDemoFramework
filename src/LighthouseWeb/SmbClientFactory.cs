using System;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace LighthouseWeb
{
    public class SmbClientFactory : ISmbClientFactory
    {
        private readonly CloudFoundryServicesOptions _services;

        public SmbClientFactory(IOptions<CloudFoundryServicesOptions> services)
        {
            _services = services.Value;
        }

        public ISmbClient GetInstance()
        {
            Console.WriteLine("Creating SMB Client");
            foreach (var service in _services.ServicesList)
            {
                if (service.Label == "user-provided" && service.Name == "win-fs-ups")
                {
                    var url = service.Credentials["url"].Value;
                    var password = service.Credentials["password"]?.Value;
                    var username = service.Credentials["username"]?.Value;
                    //var domain = service.Credentials["domain"]?.Value;
                    return new SmbClient(url, username, password);
                }
            }

            return null;
        }
    }
}