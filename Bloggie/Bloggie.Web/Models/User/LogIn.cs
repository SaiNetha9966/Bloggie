using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.User
{
    public class LogIn
    {
        [Required (ErrorMessage ="Email Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Password Required")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

    }
}
