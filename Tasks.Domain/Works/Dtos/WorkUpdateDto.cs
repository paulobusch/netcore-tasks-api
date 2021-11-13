using System;

namespace Tasks.Domain.Works.Dtos
{
    public class WorkUpdateDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Comment { get; set; }
        public float Hours { get; set; }

        public WorkUpdateDto() { }

        public WorkUpdateDto(WorkClientDto dto) : this()
        {
            Id = dto.Id;
            StartTime = dto.StartTime;
            EndTime = dto.EndTime;
            Comment = dto.Comment;
            Hours = dto.Hours;
        }
    }
}
