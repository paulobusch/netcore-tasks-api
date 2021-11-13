using System;
using Tasks.Domain._Common.Dtos;

namespace Tasks.Domain.Projects.Dtos.Works
{
    public class ProjectWorkSearchDto : PaginationDto
    {
        public Guid? DeveloperId { get; set; }
        public Guid? ProjectId { get; set; }
    }
}
