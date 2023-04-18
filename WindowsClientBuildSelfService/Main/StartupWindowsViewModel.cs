using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsClientBuildSelfService.PR.Models;
using WindowsClientBuildSelfService.PR.Services;

namespace WindowsClientBuildSelfService.Main
{
    internal class StartupWindowsViewModel:BindableBase
    {
        private readonly IPullRequestService pullRequestService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupWindowViewModel"/> class with an instance of <see cref="IPullRequestService"/>.
        /// </summary>
        /// <param name="pullRequestService">The <see cref="IPullRequestService"/> instance to use for fetching the latest pull requests.</param>
        public StartupWindowsViewModel(IPullRequestService pullRequestService)
        {
            this.pullRequestService = pullRequestService;
            LatestPullRequest();
        }

        /// <summary>
        /// Fetches the latest pull requests for the "SEPAL-3.0" repository asynchronously.
        /// </summary>
        private async void LatestPullRequest()
        {
            var result = await pullRequestService.GetPullRequests("SEPAL-3.0");
            if (result.IsSuccess)
            {
                PullRequest = new ObservableCollection<PullRequest>((result.Data as List<PullRequest>)!);
            }
            else
            {
                MessageBox.Show(result.ReasonPhrase);
                PullRequest = new ObservableCollection<PullRequest>();
            }
        }

        private ObservableCollection<PullRequest> pullRequest = new();

        /// <summary>
        /// Gets or sets the latest pull requests for the "SEPAL-3.0" repository.
        /// </summary>
        public ObservableCollection<PullRequest> PullRequest
        {
            get => pullRequest;
            set
            {
                pullRequest = value;
                RaisePropertyChanged();
            }
        }
    }
}
