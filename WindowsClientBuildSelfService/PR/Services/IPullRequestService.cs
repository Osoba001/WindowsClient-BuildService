using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsClientBuildSelfService.PR.Services
{
    /// <summary>
    /// Interface for a service that interacts with Pull Requests in a repository.
    /// </summary>
    internal interface IPullRequestService
    {
        /// <summary>
        /// Retrieves a list of Pull Requests for a given repository.
        /// </summary>
        /// <param name="repoName">The name of the repository to retrieve Pull Requests for.</param>
        /// <returns>An <see cref="ActionResult"/> indicating success or failure, with a list of <see cref="PullRequest"/> objects as the data if successful.</returns>
        Task<ActionResult> GetPullRequests(string repoName);

        /// <summary>
        /// Downloads a release from a given URL.
        /// </summary>
        /// <param name="downloadUrl">The URL of the release to download.</param>
        /// <returns>An <see cref="ActionResult"/> indicating success or failure, with the downloaded content as the data if successful.</returns>
        Task<ActionResult> DownloadRelease(string downloadUrl);

    }
}
