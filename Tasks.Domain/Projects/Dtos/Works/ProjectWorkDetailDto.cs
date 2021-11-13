using System;
using Tasks.Domain.Developers.Dtos;

namespace Tasks.Domain.Projects.Dtos.Works
{
    public class ProjectWorkDetailDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DeveloperListDto Developer { get; set; }
        public ProjectListDto Project { get; set; }
        public string Comment { get; set; }
        public float Hours { get; set; }
    }
}
