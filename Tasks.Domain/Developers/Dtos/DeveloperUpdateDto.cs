using System;
using System.ComponentModel.DataAnnotations;

namespace Tasks.Domain.Developers.Dtos
{
    public class DeveloperUpdateDto
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Login { get; set; }
    }
}
