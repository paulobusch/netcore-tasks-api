using System;
using Tasks.Domain._Common.Dtos;

namespace Tasks.Domain.Developers.Dtos.Works
{
    public class DeveloperWorkSearchDto : PaginationDto
    {
        public Guid? ProjectId { get; set; }
        public Guid DeveloperId { get; set; }

        public DeveloperWorkSearchDto(DeveloperWorkSearchClientDto dto, Guid developerId)
        {
            Page = dto.Page;
            Limit = dto.Limit;
            ProjectId = dto.ProjectId;
            DeveloperId = developerId;
        }
    }
}
