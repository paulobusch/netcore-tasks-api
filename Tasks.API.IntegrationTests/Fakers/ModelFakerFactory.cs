using Tasks.API.IntegrationTests.Fakers.Models;
namespace Tasks.API.IntegrationTests.Fakers
{
    public class ModelFakerFactory
    {
        public LoginDtoFaker LoginDto() => new();
        public PaginationDtoFaker PaginationDto() => new();
        public DeveloperCreateDtoFaker DeveloperCreateDto() => new();
        public DeveloperUpdateDtoFaker DeveloperUpdateDto() => new();
        public ProjectWorkSearchDtoFaker ProjectWorkSearchDto() => new();
        public ProjectCreateDtoFaker ProjectCreateDto() => new();
        public WorkClientDtoFaker WorkClientDto() => new();
        public ProjectUpdateDtoFaker ProjectUpdateDto() => new();
    }
}