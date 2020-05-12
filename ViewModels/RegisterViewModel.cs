using System.ComponentModel.DataAnnotations;

namespace InsideMai.ViewModels
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords must match")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
