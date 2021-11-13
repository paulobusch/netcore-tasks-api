using System;

namespace Tasks.Domain.Developers.Dtos
{
    public class DeveloperDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string CPF { get; set; }
    }
}
