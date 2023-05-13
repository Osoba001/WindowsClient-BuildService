using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WindowsClientBuildSelfService.PR.Models
{
    public class GitConfigData
    {
        public GitConfigData()
        {
            RepoBaseUri = "https://api.github.com/repos/";
            AccessToken =Unprotect("accessToken");
            Owner = "Osoba001";
            ApiVersion = "";
        }
        public string Owner { get; private set; }
        public string RepoBaseUri { get; private set; }
        public string AccessToken { get; private set; }
        public string ApiVersion { get; private set; }

        private static string Unprotect(string key)
        {
            string protectedData = ConfigurationManager.AppSettings[key];
            byte[] data = Convert.FromBase64String(protectedData);
            byte[] entropy = Encoding.ASCII.GetBytes(Assembly.GetExecutingAssembly().FullName);
            return Encoding.ASCII.GetString(ProtectedData.Unprotect(data, entropy, DataProtectionScope.CurrentUser));
        }


    }
}
