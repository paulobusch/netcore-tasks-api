using System;

namespace Tasks.Domain.Works.Dtos
{
    public class WorkCreateDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid DeveloperId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Comment { get; set; }
        public float Hours { get; set; }

        public WorkCreateDto() { }

        public WorkCreateDto(WorkClientDto dto, Guid projectId, Guid developerId) : this()
        {
            Id = dto.Id;
            StartTime = dto.StartTime;
            EndTime = dto.EndTime;
            Comment = dto.Comment;
            Hours = dto.Hours;
            ProjectId = projectId;
            DeveloperId = developerId;
        }
    }
}
