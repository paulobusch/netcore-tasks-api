using Tasks.Domain._Common.Crypto;
using Tasks.UnitTests._Common;
using Xunit;

namespace Tasks.UnitTests.Developers.Entities
{
public class DeveloperTests : BaseTest
{
    public DeveloperTests(TasksFixture fixture) : base(fixture) { }

    [Fact]
    public void HashingPasswordTest()
    {
        var password = "321654";
        var expectedHash = "1VPRSEeaJokUzst3sriOag==";

        var developer = EntitiesFactory.NewDeveloper(password: password).Get();

        Assert.Equal(expectedHash, developer.PasswordHash);
    }

    [Theory]
    [InlineData("123", "123")]
    [InlineData("123", "679875")]
    public void ValidatePasswordTest(string expected, string actual)
    {
        var developer = EntitiesFactory.NewDeveloper(password: expected).Get();
        var expectedValid = expected.Equals(actual);

        var valid = developer.ValidatePassword(actual);

        Assert.Equal(expectedValid, valid);
    }
}
}
