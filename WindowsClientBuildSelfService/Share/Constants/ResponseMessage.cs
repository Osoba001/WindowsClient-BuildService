using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsClientBuildSelfService.Share.Constants
{
    public static class ResponseMessage
    {
        public static readonly string NullResponseOnRelease = "Null response on releases";
        public static readonly string DownloadAlreadyRunning = "A download process is currently running.";
        public static readonly string ProjectAlreadyLaunch = "A project has already been launched.\nClose the launched project and try again.";
        public static readonly string NoInternetErrorMessage = "Unable to connect to server. Make sure your internet is working and try it again.";
    }
}
