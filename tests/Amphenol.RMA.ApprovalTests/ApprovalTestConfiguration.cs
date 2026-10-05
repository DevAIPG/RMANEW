using Microsoft.Extensions.Configuration;

internal static class ApprovalTestConfiguration
{
    private static readonly Lazy<(string M10, string ERP)> Connections = new(() =>
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? "Development";
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("ApplicationSettings/appsettings.json", optional: false)
            .AddJsonFile($"ApplicationSettings/appsettings.{environment}.json", optional: true);
        if (string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase))
            builder.AddUserSecrets(typeof(Amphenol.RMA.Startup).Assembly, optional: true);
        builder.AddEnvironmentVariables();
        return ApprovalTestConnection.ReadConfiguredConnections(builder.Build());
    });

    public static string M10 => Connections.Value.M10;
    public static string ERP => Connections.Value.ERP;
}
