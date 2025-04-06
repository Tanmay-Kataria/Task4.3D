using robot_controller_api.Persistence;
using robot_map_api.Persistence;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>();
// builder.Services.AddScoped<IMapDataAccess, MapADO>();
// builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandRepository>();
// builder.Services.AddScoped<IRepository, Repository>();
// builder.Services.AddScoped<IMapDataAccess, MapRepository>();
builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandEF>();
builder.Services.AddScoped<IMapDataAccess, MapEF>();
builder.Services.AddDbContext<RobotContext>();
// builder.Services.AddScoped<IMapDataAccess, MapEF>()

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
