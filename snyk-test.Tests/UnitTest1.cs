namespace snyk_test.Tests;

public class UnitTest1
{
    [Fact]
    public void BuildsUnsafeSqlWithHardcodedFakeCredential_ForSnykDemo()
    {
        // INTENTIONAL DEMO FLAW: this fake credential is hardcoded so scanners flag it.
        const string fakePassword = "FakeTestPassword-DemoOnly-456!";

        // INTENTIONAL DEMO FLAW: untrusted input is inserted directly into SQL text.
        var userInput = "bob@example.com' OR 1=1 --";
        var sql = $"SELECT * FROM Users WHERE Email = '{userInput}' AND Password = '{fakePassword}'";

        Assert.Contains("OR 1=1", sql);
        Assert.Contains(fakePassword, sql);
    }
}
