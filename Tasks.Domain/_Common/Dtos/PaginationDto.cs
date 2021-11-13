using System.ComponentModel.DataAnnotations;

namespace Tasks.Domain._Common.Dtos
{
    public class PaginationDto
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; }

        [Range(1, 100)]
        public int Limit { get; set; }

        public int CalculateOffset() => (Page - 1) * Limit;
    }
}
