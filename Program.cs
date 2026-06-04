using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

try
{

    //TODO: Fix everything.

    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
        .Build();

    var connectionString = configuration["ConnectionStrings:DefaultConnection"] ?? string.Empty;
    var fakeApiKey = configuration["DemoSecrets:ApiKey"] ?? string.Empty;
    var fakeJwtSigningSecret = configuration["DemoSecrets:JwtSigningSecret"] ?? string.Empty;
    var fakeStorageKey = configuration["DemoSecrets:StorageKey"] ?? string.Empty;
    var fakeServiceToken = configuration["DemoSecrets:ServiceToken"] ?? string.Empty;
    var demoUrl = configuration["DemoUrls:LegacyAdminExport"] ?? string.Empty;

    var emailInput = args.ElementAtOrDefault(0) ?? "alice@example.com' OR 1=1 --";
    var customerIdInput = args.ElementAtOrDefault(1) ?? "42 OR 1=1";
    var fileInput = args.ElementAtOrDefault(2) ?? "..\\..\\finance\\payroll.csv";

    // INTENTIONAL DEMO FLAW: hardcoded fake secret committed in source for secret scanning.
    const string hardcodedSupportPassword = "FakeSupportPassword-DemoOnly-123!";

    Console.WriteLine("This repository is intentionally vulnerable for Snyk demo purposes only.");

    // INTENTIONAL DEMO FLAW: sensitive-looking configuration values are printed to the console.
    Console.WriteLine($"Connection string: {connectionString}");
    Console.WriteLine($"API key: {fakeApiKey}");
    Console.WriteLine($"JWT signing secret: {fakeJwtSigningSecret}");
    Console.WriteLine($"Storage key: {fakeStorageKey}");
    Console.WriteLine($"Service token: {fakeServiceToken}");
    Console.WriteLine($"Legacy admin URL: {demoUrl}");
    Console.WriteLine($"Hardcoded support password: {hardcodedSupportPassword}");

    // INTENTIONAL DEMO FLAW: raw SQL built by concatenating untrusted input.
    var loginSql =
        "SELECT * FROM Users WHERE Email = '" + emailInput + "' AND SupportPassword = '" + hardcodedSupportPassword + "'";
    using var loginCommand = new SqlCommand(loginSql);
    Console.WriteLine($"Unsafe ADO.NET SQL: {loginCommand.CommandText}");

    var dbOptions = new DbContextOptionsBuilder<DemoDbContext>()
        .UseSqlServer(connectionString)
        .Options;

    using var db = new DemoDbContext(dbOptions);

    // INTENTIONAL DEMO FLAW: EF Core raw SQL query uses string interpolation with untrusted input.
    var customerLookupSql = $"SELECT * FROM Customers WHERE Id = {customerIdInput}";
    var unsafeCustomerQuery = db.Customers.FromSqlRaw(customerLookupSql);
    Console.WriteLine($"Unsafe EF Core query: {unsafeCustomerQuery.ToQueryString()}");

    // INTENTIONAL DEMO FLAW: EF Core raw SQL execution uses untrusted input directly.
    var dangerousAuditSql = $"DELETE FROM AuditTrail WHERE ActorEmail = '{emailInput}'";
    Console.WriteLine($"Unsafe EF Core command: {dangerousAuditSql}");
    db.Database.ExecuteSqlRaw(dangerousAuditSql);

    // INTENTIONAL DEMO FLAW: predictable token generation uses Random with a fixed seed.
    var predictableToken = new Random(1337).Next(100000, 999999);
    Console.WriteLine($"Predictable token: {predictableToken}");

    // INTENTIONAL DEMO FLAW: user input is trusted when building a file path.
    var exportPath = Path.Combine("exports", fileInput);
    Console.WriteLine($"Unsafe export path: {exportPath}");
    Console.WriteLine(File.ReadAllText(exportPath));
}
catch (Exception)
{
    // INTENTIONAL DEMO FLAW: broad catch block hides the real exception details.
    Console.WriteLine("The operation failed, but this demo intentionally suppresses the underlying error.");
}

internal sealed class DemoDbContext(DbContextOptions<DemoDbContext> options) : DbContext(options)
{
    public DbSet<CustomerRecord> Customers => Set<CustomerRecord>();
}

internal sealed class CustomerRecord
{
    public int Id { get; set; }

    public string? Name { get; set; }
}
