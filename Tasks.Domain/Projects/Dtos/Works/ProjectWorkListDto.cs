using System;

namespace Tasks.Domain.Projects.Dtos.Works
{
    public class ProjectWorkListDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public Guid DeveloperId { get; set; }
        public string DeveloperName { get; set; }
        public string Comment { get; set; }
        public float Hours { get; set; }
    }
}
