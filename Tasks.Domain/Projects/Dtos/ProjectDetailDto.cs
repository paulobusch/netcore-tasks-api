using System;
using System.Collections.Generic;
using Tasks.Domain.Developers.Dtos;

namespace Tasks.Domain.Projects.Dtos
{
    public class ProjectDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public IEnumerable<DeveloperListDto> Developers { get; set; }
    }
}
