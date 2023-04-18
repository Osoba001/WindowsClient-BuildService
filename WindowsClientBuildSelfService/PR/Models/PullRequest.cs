using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsClientBuildSelfService.PR.Models
{
    public class PullRequest
    {
        public PullRequest(string title, string Autthor, DateTime createAt, int number)
        {
            Title = title;
            Author = Autthor;
            CreatedAt = createAt;
            Number = number;
            Release = new(number);
        }
        public string? Name { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public int Number { get; set; }

        public Release Release { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
