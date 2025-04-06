using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/robot-commands")]
public class RobotCommandsController : ControllerBase
{
    private readonly IRobotCommandDataAccess _robotCommandDataAccess = new RobotCommandADO(); 

    public RobotCommandsController(IRobotCommandDataAccess robotCommandDataAccess)
    {
        _robotCommandDataAccess = robotCommandDataAccess;
    }
    private static readonly List<RobotCommand> _commands = new List<RobotCommand>
    {
        new RobotCommand(1, "PLACE", false, DateTime.Today, DateTime.Now, "places the rocket"),
        new RobotCommand(2, "LEFT", true, DateTime.Today, DateTime.Now, "moves the rocket left"),
        new RobotCommand(3, "RIGHT", true, DateTime.Today, DateTime.Now, "moves the rocket right"),
        new RobotCommand(4, "MOVE", true, DateTime.Today, DateTime.Now, "moves the rocket one unit in the direction facing")
    };

    [HttpGet()]
    public IEnumerable<RobotCommand> GetRobotCommands(){
        return  _robotCommandDataAccess.GetRobotCommands(null, null);
    }

    [HttpGet("move")]
    public IEnumerable<RobotCommand> GetMoveCommmandsOnly()
    {
        return _robotCommandDataAccess.GetMoveCommandsOnly();
    }

    [HttpGet("{id}", Name = "GetRobotCommand")]
    public IActionResult GetRobotCommandByid(int id) {
        return Ok(_robotCommandDataAccess.GetCommandByID(id));
    }

    [HttpPost()]
    public IActionResult AddRobotCommand(RobotCommand newCommand){
        if(newCommand == null)
        {
            return BadRequest();
        }

        if(_commands.Any(c => c.Name == newCommand.Name))
        {
            return Conflict("comamnd already exists.");
        }

        _robotCommandDataAccess.AddNewCommand(newCommand);

        return CreatedAtRoute("GetRobotCommand", new {id= newCommand.id}, newCommand);   
    }

    [HttpPut("{id}")]
    public IActionResult UpdateRobotCommand(int id, RobotCommand updateCommand){
        var command = _robotCommandDataAccess.GetCommandByID(id).Find(c => c.id == id);

        if (command == null)
        {
            return NotFound();
        }

        command.Name = updateCommand.Name;
        command.IsMoveCommand = updateCommand.IsMoveCommand;
        command.ModifiedDate = DateTime.Now;
        command.Description = updateCommand.Description;

        _robotCommandDataAccess.UpdateCommand(id, updateCommand);

        return CreatedAtRoute("GetRobotCommand", new {id= updateCommand.id}, updateCommand);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRobotCommand(int id){
        var command = _robotCommandDataAccess.GetCommandByID(id).Find(c => c.id == id);
        if(command == null)
        {
            return NotFound();
        }

        _robotCommandDataAccess.DeleteRobotCommand(id);
        return Content($"Command {command.Name} deleted successfully", "text/plain");
    }
}
