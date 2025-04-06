using FULLAPI.Models;
using Npgsql;
using robot_controller_api.Persistence;
namespace robot_map_api.Persistence;
//  class with data accessing methods
public class MapADO : IMapDataAccess {
// Connection string is usually set in a config file for ease of change
private const string CONNECTION_STRING =
"Host=localhost;Username=postgres;Password=1234;Database=postgres";


public  List<Map> GetMapById(int id)
{
    return GetMaps(null, id);
}

public  List<Map> GetSquareMap()
{
    bool issquaremap = true;
    return GetMaps(issquaremap, null);
}

public  List<Map> GetMaps(bool? issquare, int? id)
{
    var maps = new List<Map>();

    using var conn = new NpgsqlConnection(CONNECTION_STRING);
    conn.Open();

    var query = @"SELECT * from robotmap";

    if (id.HasValue)
    {
        query += " WHERE id = @id";
    }

    else if (issquare == true)
    {
        query += " WHERE issquare = true";
    }

    using var cmd = new NpgsqlCommand(query, conn);

    if (id.HasValue)
    {
        cmd.Parameters.AddWithValue("@id", id);
    }

    using var reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        var Id = (int)reader["id"];
        int Columns = (int)reader["columns"];
        int Rows = (int)reader["rows"];
        string description = reader["description"] as string;
        DateTime CreatedDate = (DateTime)reader["createddate"];
        DateTime ModifiedDate = (DateTime)reader["modifieddate"];

        Map map = new Map(Id, Columns, Rows, CreatedDate, ModifiedDate, description);
        maps.Add(map);
    }

    return maps;
}

public  void AddMaps(Map map)
{
    using var conn = new NpgsqlConnection(CONNECTION_STRING);
    conn.Open();

    var query = @"INSERT INTO robotmap (columns, rows, description, createddate, modifieddate)
                VALUES (@columns, @rows, @description, @createddate, @modifieddate)";

    using var cmd = new NpgsqlCommand(query, conn);
    cmd.Parameters.AddWithValue("@columns", map.Columns);
    cmd.Parameters.AddWithValue("@rows", map.Rows);
    cmd.Parameters.AddWithValue("@description", map.Description ?? (object)DBNull.Value);
    cmd.Parameters.AddWithValue("@createddate", DateTime.UtcNow);
    cmd.Parameters.AddWithValue("@modifieddate", DateTime.UtcNow);

    cmd.ExecuteNonQuery();
}

public  void UpdateMaps(int id, Map map)
{
    using var conn = new NpgsqlConnection(CONNECTION_STRING);
    conn.Open();

    var query = @"UPDATE robotmap
                    SET rows = @rows,
                        columns = @columns,
                        description = @description
                    WHERE id = @id";

    using var cmd = new NpgsqlCommand(query, conn);
    cmd.Parameters.AddWithValue("@rows", map.Rows);
    cmd.Parameters.AddWithValue("@description", map.Description ?? (object)DBNull.Value);
    cmd.Parameters.AddWithValue("@columns", map.Columns);
    cmd.Parameters.AddWithValue("@id", id);

    cmd.ExecuteNonQuery();
}

public  void DeleteMaps(int id)
{
    using var conn = new NpgsqlConnection(CONNECTION_STRING);
    conn.Open();

    var query = @"DELETE FROM robotmap
                WHERE id = @id";
    
    using var cmd = new NpgsqlCommand(query, conn);
    cmd.Parameters.AddWithValue("@id", id);

    cmd.ExecuteNonQuery();
}




}

