using Npgsql;
namespace robot_controller_api.Persistence;
//  class with data accessing methods
public  class RobotCommandADO : IRobotCommandDataAccess {
// Connection string is usually set in a config file for ease of change
private const string CONNECTION_STRING =
"Host=localhost;Username=postgres;Password=1234;Database=postgres";

public  List<RobotCommand> GetMoveCommandsOnly()
{
    bool movecommand = true;
    return GetRobotCommands(movecommand, null);
}

public  List<RobotCommand> GetCommandByID(int id)
{
    return GetRobotCommands(null, id);
}

public  List<RobotCommand> GetRobotCommands(bool? ismovecommand, int? Id) {
    var robotCommands = new List<RobotCommand>();

    // Connect to our Postgres database
    using var conn = new NpgsqlConnection(CONNECTION_STRING);
    conn.Open();

    var query = "SELECT * FROM robotcommand";

    if (ismovecommand == true)
    {
        query+= " WHERE ismovecommand = true";
    }

    else if(Id.HasValue)
    {
        query += " WHERE id = @id";
    }

    // Execute a SQL query and read the return result(s)
    using var cmd = new NpgsqlCommand(query, conn);

    if (Id.HasValue)
    {
        cmd.Parameters.AddWithValue("@id", Id);
    }


    using var reader = cmd.ExecuteReader();
    while (reader.Read()) {

    var id = (int)reader["id"];
    string description = reader["description"] as string;
    bool isMoveCommand = (bool)reader["ismovecommand"];
    string Name = (string)reader["name"];
    DateTime CreatedDate = (DateTime)reader["createddate"];
    DateTime ModifiedDate = (DateTime)reader["modifieddate"];
    

    // Create a new robotCommand object
    // Add each command to our list
        
    RobotCommand robotCommand = new RobotCommand(id, Name, isMoveCommand, CreatedDate, ModifiedDate, description);
    robotCommands.Add(robotCommand);
    }
    return robotCommands; 
}


public  void AddNewCommand(RobotCommand robotCommand)
{
    using var conn = new NpgsqlConnection(CONNECTION_STRING);
    conn.Open();

    var query = @"INSERT INTO robotcommand (name, description, ismovecommand, createddate, modifieddate)
                VALUES (@name, @description, @ismovecommand, @createddate, @modifieddate)";

    using var cmd = new NpgsqlCommand(query, conn);
    cmd.Parameters.AddWithValue("@name", robotCommand.Name);
    cmd.Parameters.AddWithValue("@ismovecommand", robotCommand.IsMoveCommand);
    cmd.Parameters.AddWithValue("@description", robotCommand.Description ?? (object)DBNull.Value);
    cmd.Parameters.AddWithValue("@createddate", DateTime.UtcNow);
    cmd.Parameters.AddWithValue("@modifieddate", DateTime.UtcNow);

    cmd.ExecuteNonQuery();
}

public  void UpdateCommand(int id, RobotCommand robotCommand)
{
    using var conn = new NpgsqlConnection(CONNECTION_STRING);
    conn.Open();

    var query = @"UPDATE robotcommand
                    SET name = @name,
                        description = @description,
                        ismovecommand = @ismovecommand
                    WHERE id = @id";

    using var cmd = new NpgsqlCommand(query, conn);
    cmd.Parameters.AddWithValue("@name", robotCommand.Name);
    cmd.Parameters.AddWithValue("@description", robotCommand.Description);
    cmd.Parameters.AddWithValue("@ismovecommand", robotCommand.IsMoveCommand);
    cmd.Parameters.AddWithValue("@id", id);

    cmd.ExecuteNonQuery();
}

public  void DeleteRobotCommand(int id)
{
    using var conn = new NpgsqlConnection(CONNECTION_STRING);
    conn.Open();

    var query = @"DELETE FROM robotcommand
                WHERE id = @id";
    
    using var cmd = new NpgsqlCommand(query, conn);
    cmd.Parameters.AddWithValue("@id", id);

    cmd.ExecuteNonQuery();
}



}