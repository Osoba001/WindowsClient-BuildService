using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsClientBuildSelfService.PR.Models;

namespace WindowsClientBuildSelfService.PR.Response
{
    public class PullRequestResponse
    {

        public int number { get; set; }
        public string title { get; set; }
        public User user { get; set; }
        public DateTime created_at { get; set; }


        public static implicit operator PullRequest(PullRequestResponse dto)
        {
            return new PullRequest(dto.title, dto.user.login, dto.created_at, dto.number);
        }
    }

    public class User
    {
        public string login { get; set; }

    }
}
