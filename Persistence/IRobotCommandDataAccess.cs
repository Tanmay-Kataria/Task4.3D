namespace robot_controller_api.Persistence;
public interface IRobotCommandDataAccess {
// Method signatures from your RobotCommandDataAccess class go here.
// Your method signatures may differ, but may look something like:
List<RobotCommand> GetRobotCommands(bool? ismovecommand, int? Id);

List<RobotCommand> GetMoveCommandsOnly();
List<RobotCommand> GetCommandByID(int id);
void AddNewCommand(RobotCommand newCommand);
void UpdateCommand(int id, RobotCommand newCommand);
void DeleteRobotCommand(int id);
}