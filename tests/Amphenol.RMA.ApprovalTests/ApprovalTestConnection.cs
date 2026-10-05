using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

internal static class ApprovalTestConnection
{
    public static (string M10, string ERP) ReadConfiguredConnections(IConfiguration configuration)
    {
        string Read(string name) => configuration.GetConnectionString(name)
            ?? throw new InvalidOperationException($"Missing application connection string: {name}.");
        // Validate both destinations before allowing either database to be used.
        return (Validate(Read("ConnectionM10")), Validate(Read("Connection500")));
    }

    public static string Validate(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var server = builder.DataSource.Trim();
        if (server.StartsWith("tcp:", StringComparison.OrdinalIgnoreCase))
            server = server.Substring(4);
        // Permit instances and ports on the explicitly approved test hosts.
        var host = server.Split('\\', ',')[0];
        if (!string.Equals(host, "AIO-POS", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(host, "M10TestNA01", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "Approval integration tests may connect only to AIO-POS or M10TestNA01.");
        if (string.IsNullOrWhiteSpace(builder.InitialCatalog))
            throw new InvalidOperationException("Specify the test database name in the connection string.");
        return connectionString;
    }
}
