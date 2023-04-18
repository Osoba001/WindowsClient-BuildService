using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsClientBuildSelfService.Share
{
    /// <summary>
    /// Represents the result of an action that can be successful or contain error messages.
    /// </summary>
    public class ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionResult"/> class with default values.
        /// </summary>
        public ActionResult()
        {
            ErrorMessagesList = new List<string>();
            IsSuccess = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the action was successful or not.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the data returned by the action if successful.
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// Gets the reason phrase containing error messages if the action was not successful.
        /// </summary>
        public string ReasonPhrase => Errors();

        private List<string> ErrorMessagesList { get; set; }

        /// <summary>
        /// Adds an error message to the list of error messages and sets <see cref="IsSuccess"/> to <c>false</c>.
        /// </summary>
        /// <param name="error">The error message to add.</param>
        public void AddError(string error)
        {
            ErrorMessagesList.Add(error);
            IsSuccess = false;
        }

        private string Errors()
        {
            StringBuilder er = new();
            foreach (var error in ErrorMessagesList)
            {
                er.AppendLine(error);
            }
            return er.ToString();
        }
    }
}
