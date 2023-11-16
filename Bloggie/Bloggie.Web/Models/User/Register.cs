using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.User
{
    public class Register :IdentityUser
    {
        [Required (ErrorMessage ="First Name Required") ]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "+" +
            " Name Required")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Email Required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required (ErrorMessage ="Password Required")]
        public string Password { get; set; }

    }
}
