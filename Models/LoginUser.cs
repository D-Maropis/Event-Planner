using System.ComponentModel.DataAnnotations;

namespace ExamOne.Models
{
    public class LoginUser
    {
        [Required]
        [EmailAddress(ErrorMessage="Email is required.")]
        [Display(Name="Email: ")]
        public string LoginEmail {get; set;}

        [Required(ErrorMessage="Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name="Password: ")]
        public string LoginPassword {get; set;}
    }
}