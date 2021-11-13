using Tasks.API.IntegrationTests.Assertions.Comparators;
namespace Tasks.API.IntegrationTests.Assertions
{
    public class CompareFactory
    {
        public DeveloperEqualsDeveloperListDto DeveloperEqualsDeveloperListDto() => new();
        public DeveloperEqualsDeveloperDetailDto DeveloperEqualsDeveloperDetailDto() => new();
        public DeveloperEqualsDeveloperCreateDto DeveloperEqualsDeveloperCreateDto() => new();
        public DeveloperEqualsDeveloperUpdateDto DeveloperEqualsDeveloperUpdateDto() => new();
        public ProjectEqualsProjectDetailDto ProjectEqualsProjectDetailDto() => new();
        public ProjectEqualsProjectListDto ProjectEqualsProjectListDto() => new();
        public ProjectEqualsProjectWorkListDto ProjectEqualsProjectWorkListDto() => new();
        public ProjectEqualsProjectWorkDetailDto ProjectEqualsProjectWorkDetailDto() => new();
        public ProjectEqualsProjectCreateDto ProjectEqualsProjectCreateDto() => new();
        public ProjectEqualsWorkClientDto ProjectEqualsWorkClientDto() => new();
        public ProjectEqualsProjectUpdateDto ProjectEqualsProjectUpdateDto() => new();
    }
}