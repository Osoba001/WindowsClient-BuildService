using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WindowsClientBuildSelfService.PR.Models;
using WindowsClientBuildSelfService.PR.Response;

namespace WindowsClientBuildSelfService.PR.Services
{
    /// <summary>
    /// Implements the IPullRequestService interface to provide functionality related to pull requests.
    /// </summary>
    public class PullRequestService : IPullRequestService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the PullRequestService class.
        /// </summary>
        public PullRequestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Downloads a release from the specified download URL.
        /// </summary>
        /// <param name="downloadUrl">The URL from which to download the release.</param>
        /// <returns>An ActionResult object containing the downloaded content if the download was successful, or an error message if the download failed.</returns>
        public async Task<ActionResult> DownloadRelease(string downloadUrl)
        {
            var result = new ActionResult();
            var resp = await _httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
            if (resp.IsSuccessStatusCode)
            {
                result.Data = resp.Content;
            }
            else
                result.AddError(resp.ReasonPhrase ?? "");
            return result;
        }

        /// <summary>
        /// Retrieves a list of open pull requests for the specified repository.
        /// </summary>
        /// <param name="repoName">The name of the repository for which to retrieve pull requests.</param>
        /// <returns>An ActionResult object containing a list of PullRequest objects if the retrieval was successful, or an error message if the retrieval failed.</returns>
        public async Task<ActionResult> GetPullRequests(string repoName)
        {
            var resp = await _httpClient.GetAsync($"{repoName}/pulls?state=open");
            var result = new ActionResult();
            if (!resp.IsSuccessStatusCode)
            {
                result.AddError(resp.ReasonPhrase ?? "");
                return result;
            }
            var pullRequests = await resp.Content.ReadFromJsonAsync<List<PullRequestResponse>>();
            if (pullRequests == null)
            {
                result.AddError("Null response");
                return result;
            }

            pullRequests = pullRequests.OrderByDescending(x => x.created_at).ToList();
            return await AddDownloadUrl(repoName, ConverPRsResponseToModel(pullRequests));
        }

        /// <summary>
        /// Adds the download URL for each pull request's associated release.
        /// </summary>
        /// <param name="repoName">The name of the repository for which to retrieve releases.</param>
        /// <param name="prs">The list of PullRequest objects to update.</param>
        /// <returns>An ActionResult object containing the updated list of PullRequest objects.</returns>
        private async Task<ActionResult> AddDownloadUrl(string repoName, List<PullRequest> prs)
        {
            var result = new ActionResult();
            var resp = await _httpClient.GetAsync($"{repoName}/releases");
            if (!resp.IsSuccessStatusCode)
            {
                result.AddError(resp.ReasonPhrase ?? "");
                return result;
            }
            var releases = await resp.Content.ReadFromJsonAsync<List<ReleaseResponse>>();
            if (releases is null)
            {
                result.AddError(NullResponseOnRelease);
                return result;
            }
            prs.ForEach(pr =>
            {
                var asset = releases.Where(r => r.tag_name == $@"refs/pull/{pr.Number}/merge").FirstOrDefault()?.assets.FirstOrDefault();
                pr.Release.DownloadReleaseUrl = asset?.browser_download_url;
                pr.Status = pr.Release.DownloadReleaseUrl != null ? "Approved" : "Pending";
                pr.Name = repoName;
            });

            result.Data = prs;
            return result;
        }

        private List<PullRequest> ConverPRsResponseToModel(List<PullRequestResponse> responses)
        {
            List<PullRequest> models = new();
            foreach (var pr in responses)
            {
                models.Add(pr);
            }
            return models;
        }
    }
}
