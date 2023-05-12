using Microsoft.Extensions.DependencyInjection;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WindowsClientBuildSelfService.PR.Models;
using WindowsClientBuildSelfService.PR.Services;
using WindowsClientBuildSelfService.Share.Messages;

namespace WindowsClientBuildSelfService.Main
{
    internal class StartupWindowsViewModel:BindableBase
    {
        private readonly IPullRequestService pullRequestService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupWindowViewModel"/> class with an instance of <see cref="IPullRequestService"/>.
        /// </summary>
        /// <param name="pullRequestService">The <see cref="IPullRequestService"/> instance to use for fetching the latest pull requests.</param>
        private IMessages msgSender;
 
        public StartupWindowsViewModel()
        {
            pullRequestService = App.serviceProvider.GetRequiredService<IPullRequestService>();
            msgSender = App.serviceProvider.GetRequiredService<IMessages>();
            LatestPullRequest().Await();

        }

        /// <summary>
        /// Fetches the latest pull requests for the "SEPAL-3.0" repository asynchronously.
        /// </summary>
        private async Task LatestPullRequest()
        {
            var result = await pullRequestService.GetPullRequests("SMBS");
            if (result.IsSuccess)
            {
                PullRequest = new ObservableCollection<PullRequest>((result.Data as List<PullRequest>)!);
            }
            else
            {
                msgSender.ShowMessage(result.ReasonPhrase);
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
