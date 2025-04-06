using Npgsql;
namespace robot_controller_api.Persistence;

// Interface
public interface IRepository
{
    List<T> ExecuteReader<T>(string sqlCommand, NpgsqlParameter[]? dbParams = null) where T : class, new();
}

// Implementation
public class Repository : IRepository
{
    private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=1234;Database=postgres";

    public List<T> ExecuteReader<T>(string sqlCommand, NpgsqlParameter[]? dbParams = null) where T : class, new()
    {
        var entities = new List<T>();
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand(sqlCommand, conn);
        if (dbParams is not null)
        {
            cmd.Parameters.AddRange(dbParams.Where(x => x.Value is not null).ToArray());
        }

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var entity = new T();
            reader.MapTo(entity);
            entities.Add(entity);
        }
        return entities;
    }
}
