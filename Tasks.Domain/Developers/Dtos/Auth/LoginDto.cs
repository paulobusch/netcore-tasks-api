using System.ComponentModel.DataAnnotations;

namespace Tasks.Domain.Developers.Dtos.Auth
{
    public class LoginDto : LoginBase
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
