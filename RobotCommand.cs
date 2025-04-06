using System.ComponentModel.DataAnnotations;

namespace robot_controller_api;

public class RobotCommand
{
    [Key]
    public int id {get; set;}
    public string Name{get; set;}
    public string? Description{get; set;}
    public bool IsMoveCommand{get; set;}
    public DateTime CreatedDate {get; set;}
    public DateTime ModifiedDate{get; set;}

    public RobotCommand() {}

    public RobotCommand (
        int Id,
        string name,
        bool isMoveCommand,
        DateTime createdDate,
        DateTime modifiedDate,
        string description = null)
        {
            id = Id;
            Name = name;
            IsMoveCommand = isMoveCommand;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
            Description = description;
        }
}
