using Npgsql;
using robot_controller_api;
using robot_controller_api.Persistence;

public class RobotCommandRepository : IRobotCommandDataAccess
{
    private IRepository _repo;

    public RobotCommandRepository(IRepository repository)
    {
        _repo = repository;
    }

    public  List<RobotCommand> GetMoveCommandsOnly()
    {
        bool movecommand = true;
        return GetRobotCommands(movecommand, null);
    }

public  List<RobotCommand> GetCommandByID(int id)
    {
        return GetRobotCommands(null, id);
    }


    public List<RobotCommand> GetRobotCommands(bool? ismovecommand, int? Id)
    {
        string query = "SELECT * FROM public.robotcommand";
        var param = new NpgsqlParameter[] {
            new ("id", Id),
            
        };
        if (ismovecommand == true)
        {
            query+= " WHERE ismovecommand = true";
        }

        else if(Id.HasValue)
        {
            query += " WHERE id = @id";
        }
        return _repo.ExecuteReader<RobotCommand>(query, param);
    }

    public void UpdateCommand(int id, RobotCommand robotCommand)
    {
        var param = new NpgsqlParameter[] {
            new ("id", id),
            new("name", robotCommand.Name),
            new ("description", robotCommand.Description ?? (object)DBNull.Value),
            new ("ismovecommand", robotCommand.IsMoveCommand)
        };
        var query = @"UPDATE robotcommand
                    SET name = @name,
                        description = @description,
                        ismovecommand = @ismovecommand
                    WHERE id = @id RETURNING *";

        
        _repo.ExecuteReader<RobotCommand>(query, param).Single();

        
    }

    public void AddNewCommand(RobotCommand robotCommand)
    {
        var param = new NpgsqlParameter[] {
            new ("name", robotCommand.Name),
            new ("ismovecommand", robotCommand.IsMoveCommand),
            new ("description", robotCommand.Description ?? (object)DBNull.Value),
            new ("createddate", DateTime.UtcNow),
            new ("modifieddate", DateTime.UtcNow)
        };
        var query = @"INSERT INTO robotcommand (name, description, ismovecommand, createddate, modifieddate)
                VALUES (@name, @description, @ismovecommand, @createddate, @modifieddate)
                RETURNING id";

        _repo.ExecuteReader<RobotCommand>(query, param).Single();
    }

    public void DeleteRobotCommand(int id)
    {
        var param = new NpgsqlParameter[] {
            new ("id", id),
        };

        var query = @"DELETE FROM robotcommand
                WHERE id = @id";

        _repo.ExecuteReader<RobotCommand>(query,param);
    }
}
