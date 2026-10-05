using Microsoft.Data.SqlClient;

internal static class ApprovalTestConnection
{
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
