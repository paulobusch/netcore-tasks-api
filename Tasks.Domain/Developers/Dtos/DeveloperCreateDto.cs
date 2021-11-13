using System;
using System.ComponentModel.DataAnnotations;

namespace Tasks.Domain.Developers.Dtos
{
    public class DeveloperCreateDto
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Login { get; set; }

        [Required]
        [StringLength(11)]
        public string CPF { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
