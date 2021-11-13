using System;
using System.Collections.Generic;
using Tasks.Domain._Common.Enums;

namespace Tasks.Domain.Developers.Dtos
{
    public class LoginBase
    {
        public Status Status { get; set; }
        public int[] Ids { get; set; }
        public IEnumerable<Guid> Keys { get; set; }
        public List<DeveloperCreateDto> Models { get; set; }
        public DeveloperUpdateDto Update { get; set; }
    }
}
