using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsClientBuildSelfService.PR.Response
{
    public class ReleaseResponse
    {
        public string tag_name { get; set; }
        public Asset[] assets { get; set; }
    }
    public class Asset
    {
        public string browser_download_url { get; set; }
        public string name { get; set; }
    }
}
