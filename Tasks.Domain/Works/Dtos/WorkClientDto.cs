using System;
using System.ComponentModel.DataAnnotations;

namespace Tasks.Domain.Works.Dtos
{
    public class WorkClientDto
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [MaxLength(300)]
        public string Comment { get; set; }

        [Required]
        [Range(0.1, 3000)]
        public float Hours { get; set; }
    }
}
