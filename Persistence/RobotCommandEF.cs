using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

class RobotCommandEF : IRobotCommandDataAccess
{
    private RobotContext _context;

    public RobotCommandEF(RobotContext context)
    {
        _context = context;
    }

    public List<RobotCommand> GetMoveCommandsOnly()
    {
        return GetRobotCommands(true, null);
    }

    public List<RobotCommand> GetCommandByID(int id)
    {
        return GetRobotCommands(null, id);
    }

    public  List<RobotCommand> GetRobotCommands(bool? ismovecommand, int? Id)
    {
        var query = _context.Robotcommands.AsQueryable();

        if (ismovecommand.HasValue)
        {
            query = query.Where(cmd => cmd.IsMoveCommand == ismovecommand.Value);
        }

        else if(Id.HasValue)
        {
            query = query.Where(cmd => cmd.id.Equals(Id));
        }

        return  query.ToList();
    }


    public void UpdateCommand (int id, RobotCommand robotCommand)
    {
        var existingcmd = _context.Robotcommands.FirstOrDefault(cmd => cmd.id.Equals(id));
        if (existingcmd != null)
        {
            existingcmd.Name = robotCommand.Name;
            existingcmd.Description = robotCommand.Description;
            existingcmd.IsMoveCommand = robotCommand.IsMoveCommand;
            existingcmd.ModifiedDate = robotCommand.ModifiedDate;   

            _context.SaveChanges();
        }
    }

    public void AddNewCommand(RobotCommand newCommand)
    {
        newCommand.CreatedDate = DateTime.Today;
        newCommand.ModifiedDate = DateTime.Today;
        _context.Robotcommands.Add(newCommand);
        _context.SaveChanges();
    }

    public void DeleteRobotCommand(int id)
    {
        var existingcmd = _context.Robotcommands.Find(id);
        if (existingcmd != null)
        {
            _context.Robotcommands.Remove(existingcmd);
            _context.SaveChanges();
        }
    }
} 