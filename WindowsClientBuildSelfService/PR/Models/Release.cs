using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WindowsClientBuildSelfService.PR.Services;
using WindowsClientBuildSelfService.Share.Messages;

namespace WindowsClientBuildSelfService.PR.Models
{
    public class Release : BindableBase
    {
        private IMessages msgSender;
        private const string destinationPath = "C:/ProgramData/CypherCrescent/builds";
        private readonly IPullRequestService gitRepo;
        public Release(int prNumber)
        {
            gitRepo = App.serviceProvider.GetRequiredService<IPullRequestService>();
            msgSender= App.serviceProvider.GetRequiredService<IMessages>();
            DownloadPRRelease = new DelegateCommand(DownloadRelease, CanDownload);
            IsDownloadInProgress = false;
            PRNumber = prNumber;
        }
        public string? DownloadReleaseUrl { get; set; }
        public int PRNumber { get; set; }
        private long downloadProgress;

        public long DownloadProgress
        {
            get { return downloadProgress; }
            set
            {
                downloadProgress = value;
                RaisePropertyChanged();
            }
        }


        public DelegateCommand DownloadPRRelease { get; set; }
        public async void DownloadRelease()
        {
            var result= CreateDiretoryIfNotExist(destinationPath);
            if (!result.IsSuccess)
            {
                msgSender.ShowMessage(result.ReasonPhrase);
                return;
            }
            if (IsDownloadInProgress)
            {
                msgSender.ShowMessage(DownloadAlreadyRunning);
                return;
            }

            try
            {
                var exeFile = new DirectoryInfo(destinationPath).GetFiles("*.exe").FirstOrDefault();
                if (exeFile != null)
                    using (FileStream stream = exeFile.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Dispose();
                    }
            }
            catch (IOException)
            {
                msgSender.ShowMessage(ProjectAlreadyLaunch);
                return;
            }
            catch(Exception ex)
            {
                msgSender.ShowMessage(ex.Message);
                return;
            }
            if (!CheckIfPRAlreadyExist())
            {
                result = DeletFileIfExist(destinationPath); ;
                if (!result.IsSuccess)
                {
                    msgSender.ShowMessage(result.ReasonPhrase);
                    return;
                }
                await DownloadFile();
            }


        }
        public bool CanDownload()
        {
            return DownloadReleaseUrl is not null;
        }
        private async Task DownloadFile()
        {
            IsDownloadInProgress = true;
            var result = await gitRepo.DownloadRelease(DownloadReleaseUrl ?? "");
            if (!result.IsSuccess)
            {
                IsDownloadInProgress = false;
                msgSender.ShowMessage(result.ReasonPhrase);
                return;
            }
            var httpContent = result.Data as HttpContent;

            var totalBytes = httpContent!.Headers.ContentLength ?? -1L;
            var readBytes = 0L;

            using var contentStream = await httpContent.ReadAsStreamAsync();
            var fullPath = Path.Join(destinationPath, PRNumber + DownloadReleaseUrl!.Split("/")[^1]);
            using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
            var buffer = new byte[4096];
            var isMoreToRead = true;

            do
            {
                var bytesRead = await contentStream.ReadAsync(buffer);
                if (bytesRead == 0)
                {
                    isMoreToRead = false;
                    continue;
                }
                try
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                }
                catch (Exception ex)
                {
                    isMoreToRead = false;
                    msgSender.ShowMessage(ex.Message);
                    continue;
                }

                readBytes += bytesRead;
                DownloadProgress = (readBytes * 100)/ totalBytes;
            }
            while (isMoreToRead);
            IsDownloadInProgress = false;
        }
        internal static ActionResult CreateDiretoryIfNotExist(string path)
        {
            var result = new ActionResult();    
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (UnauthorizedAccessException)
                {
                    result.AddError(UnauthorizedDirectoryMsg(path));
                }
                catch(Exception e)
                {
                    result.AddError($"{e.Message}");
                }
            }
            return result;
        }
        public ActionResult DeletFileIfExist(string path)
        {
            var result=new ActionResult();
            DirectoryInfo directory = new(path);
            IsDownloadInProgress = true;
            foreach (FileInfo file in directory.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (UnauthorizedAccessException)
                {
                    result.AddError(UnauthorizedFileMsg(file.FullName));
                }
                catch (Exception e)
                {
                    result.AddError($"{e.Message}");
                }
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch (Exception e)
                {
                    result.AddError($"{e.Message}");
                }
            }
            IsDownloadInProgress = false;
            return result;
        }
        private static bool IsDownloadInProgress = false;

        private bool CheckIfPRAlreadyExist()
        {
            try
            {
                var exeFile = new DirectoryInfo(destinationPath).GetFiles("*.exe").FirstOrDefault();
                if (exeFile == null)
                    return false;
                if (exeFile.Name != $"{PRNumber}{DownloadReleaseUrl!.Split("/")[^1]}")
                {
                    return false;
                }
                try
                {
                    Process.Start(exeFile.FullName);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                msgSender.ShowMessage(e.Message);
                return true;
            }
        }
    }
}
