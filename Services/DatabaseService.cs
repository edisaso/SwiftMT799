using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

public class DatabaseService(IConfiguration configuration)
{
#pragma warning disable CS8601 // Possible null reference assignment.
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");
#pragma warning restore CS8601 // Possible null reference assignment.

    public async Task InitializeDatabaseAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS SwiftMT799Messages (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Sender TEXT,
                Receiver TEXT,
                TransactionReference TEXT,
                RelatedReference TEXT,
                NarrativeText TEXT
            )";
        await command.ExecuteNonQueryAsync();
    }

    public async Task SaveMessageAsync(SwiftMT799Message message)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO SwiftMT799Messages (Sender, Receiver, TransactionReference, RelatedReference, NarrativeText)
            VALUES (@Sender, @Receiver, @TransactionReference, @RelatedReference, @NarrativeText)";

        command.Parameters.AddWithValue("@Sender", message.Sender);
        command.Parameters.AddWithValue("@Receiver", message.Receiver);
        command.Parameters.AddWithValue("@TransactionReference", message.TransactionReference);
        command.Parameters.AddWithValue("@RelatedReference", message.RelatedReference);
        command.Parameters.AddWithValue("@NarrativeText", message.NarrativeText);

        await command.ExecuteNonQueryAsync();
    }
}
