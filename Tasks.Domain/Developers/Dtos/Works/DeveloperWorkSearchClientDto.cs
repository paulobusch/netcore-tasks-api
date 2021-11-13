using System;
using Tasks.Domain._Common.Dtos;

namespace Tasks.Domain.Developers.Dtos.Works
{
    public class DeveloperWorkSearchClientDto : PaginationDto
    {
        public Guid? ProjectId { get; set; }
    }
}
