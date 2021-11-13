using System;
using Tasks.Domain.Projects.Dtos;

namespace Tasks.Domain.Developers.Dtos.Works
{
    public class DeveloperWorkListDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ProjectListDto Project { get; set; }
        public string Comment { get; set; }
        public float Hours { get; set; }
    }
}
