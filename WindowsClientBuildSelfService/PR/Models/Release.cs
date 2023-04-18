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
using System.Windows;
using WindowsClientBuildSelfService.PR.Services;

namespace WindowsClientBuildSelfService.PR.Models
{
    public class Release : BindableBase
    {
        private const string destinationPath = "C:/ProgramData/CypherCrescent/builds";
        public Release(int prNumber)
        {
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
            CreateDiretoryIfNotExist(destinationPath);
            if (IsDownloadInProgress)
            {
                MessageBox.Show(DownloadAlreadyRunning);
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
                MessageBox.Show(ProjectAlreadyLaunch);
                return;
            }
            if (!CheckIfPRAlreadyExist())
            {
                DeletFileIfExist(destinationPath);
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
            var gitRepo = App.serviceProvider.GetRequiredService<IPullRequestService>();
            var result = await gitRepo.DownloadRelease(DownloadReleaseUrl ?? "");
            if (!result.IsSuccess)
            {
                MessageBox.Show(result.ReasonPhrase);
                IsDownloadInProgress = false;
                return;
            }
            var httpContent = result.Data as HttpContent;

            var totalBytes = httpContent!.Headers.ContentLength ?? -1L;
            var readBytes = 0L;

            using var contentStream = await httpContent.ReadAsStreamAsync();
            var fullPath = Path.Join(destinationPath, PRNumber + DownloadReleaseUrl!.Split("/")[^1]);
            using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
            var buffer = new byte[40960];
            var isMoreToRead = true;

            do
            {
                var bytesRead = await contentStream.ReadAsync(buffer);
                if (bytesRead == 0)
                {
                    isMoreToRead = false;
                    continue;
                }
                await fileStream.WriteAsync(buffer, 0, bytesRead);

                readBytes += bytesRead;
                DownloadProgress = readBytes * 100 / totalBytes;
            }
            while (isMoreToRead);
            IsDownloadInProgress = false;
        }
        private void CreateDiretoryIfNotExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        private void DeletFileIfExist(string path)
        {

            DirectoryInfo directory = new(path);
            IsDownloadInProgress = true;
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }
            IsDownloadInProgress = false;
        }
        private static bool IsDownloadInProgress = false;

        private bool CheckIfPRAlreadyExist()
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
    }
}
