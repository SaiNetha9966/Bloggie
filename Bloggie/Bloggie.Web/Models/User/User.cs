using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.User
{
    public class User
    {
        public string Id { get; set; } 
        [Required  (ErrorMessage ="Firstname required")]
        public string Firstname { get; set; }
       
        [Required(ErrorMessage = "Lastname required")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Email required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username required")]
        public string Username {  get; set; }   

        [Required(ErrorMessage = "Password required")]
        [MinLength(6, ErrorMessage = "Password has to be at least 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phonenumber required")]

        public string PhoneNumber { get; set; } 
    }
}
