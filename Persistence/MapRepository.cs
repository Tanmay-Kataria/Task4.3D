using FULLAPI.Models;
using Npgsql;
using robot_controller_api.Persistence;

public class MapRepository : IMapDataAccess
{
    private IRepository _repo;

    public MapRepository(IRepository repository)
    {
        _repo = repository;
    }

    public  List<Map> GetMaps(bool? issquare, int? id)
    {
        var query = @"SELECT * from robotmap";
        var param = new NpgsqlParameter[]
        {
            new ("id", id)
        };

        if (id.HasValue)
        {
            query += " WHERE id = @id";
        }

        else if (issquare == true)
        {
            query += " WHERE issquare = true";
        }
        return _repo.ExecuteReader<Map>(query, param);
    }

    public  List<Map> GetMapById(int id)
    {
        return GetMaps(null, id);
    }

    public  List<Map> GetSquareMap()
    {
        bool issquaremap = true;
        return GetMaps(issquaremap, null);
    }

    public  void AddMaps(Map map)
    {
        var query = @"INSERT INTO robotmap (columns, rows, description, createddate, modifieddate)
                VALUES (@columns, @rows, @description, @createddate, @modifieddate)
                RETURNING id";

        var param = new NpgsqlParameter[] {
            new ("columns", map.Columns),
            new ("rows", map.Rows),
            new ("description", map.Description ?? (object)DBNull.Value),
            new ("createddate", DateTime.UtcNow),
            new ("modifieddate", DateTime.UtcNow)
        };

        _repo.ExecuteReader<Map>(query, param);

    }

    public  void UpdateMaps(int id, Map map)
    {
        var query = @"UPDATE robotmap
                    SET rows = @rows,
                        columns = @columns,
                        description = @description
                    WHERE id = @id";

        var param = new NpgsqlParameter[] {
            new ("id", id),
            new ("columns", map.Columns),
            new ("rows", map.Rows),
            new ("description", map.Description ?? (object)DBNull.Value),
        };
        _repo.ExecuteReader<Map>(query, param);
    }

    public  void DeleteMaps(int id)
    {
        var query = @"DELETE FROM robotmap
                WHERE id = @id";

        var param = new NpgsqlParameter[] {
            new ("id", id),
        };

        _repo.ExecuteReader<Map>(query, param);
    }
 
}