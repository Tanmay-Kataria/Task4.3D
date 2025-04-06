using FULLAPI.Models;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;
using robot_map_api.Persistence;

namespace FULLAPI.Controllers;

[ApiController]
[Route("api/maps")]
public class MapsController : ControllerBase
{
    private readonly IMapDataAccess _mapDataAccess;

    public MapsController(IMapDataAccess mapDataAccess)
    {
        _mapDataAccess = mapDataAccess;
    }
    private static readonly List<Map> _maps = new List<Map>
    {
        new Map(1,2,3, DateTime.Now, DateTime.Now, "Small  Map"),
        new Map(2,8,9, DateTime.Now, DateTime.Now, "Medium Map"),
        new Map(3,12,15, DateTime.Now, DateTime.Now, "Large Map"),
        new Map(4,20, 30, DateTime.Today, DateTime.Now, "Huge map")
    };

    [HttpGet]
    public IEnumerable<Map> GetAllMaps() {
        return _mapDataAccess.GetMaps(null, null);
    }

    [HttpGet("square")]
    public IEnumerable<Map> GetSquareMaps(){
        return _mapDataAccess.GetSquareMap();
    }

    [HttpGet("{id}", Name = "GetMap")]
    public IActionResult GetMapById(int id) {
        
        return Ok(_mapDataAccess.GetMapById(id));
    }

     [HttpPost()]
    public IActionResult AddMap(Map newMap){
        if(newMap == null)
        {
            return BadRequest();
        }

        if(_mapDataAccess.GetMaps(null, null).Find(m => m.Id == newMap.Id) != null)
        {
            return Conflict($"map with id {newMap.Id} already exists.");
        }
        
        newMap.CreatedDate = DateTime.Now;
        newMap.ModifiedDate = DateTime.Now;

        _mapDataAccess.AddMaps(newMap);

        return CreatedAtRoute("GetMap", new {id= newMap.Id}, newMap);   
    }

    [HttpPut("{id}")]
public IActionResult UpdateMap(int id, [FromBody] Map updateMap)
{
    if (updateMap == null)
    {
        return BadRequest("Invalid map data.");
    }

    var map = _mapDataAccess.GetMapById(id).Find(m => m.Id == id);
    if (map == null)
    {
        return NotFound($"Map with ID {id} not found.");
    }

    map.Columns = updateMap.Columns;
    map.Rows = updateMap.Rows;
    map.Description = updateMap.Description;
    map.ModifiedDate = DateTime.Now;

    _mapDataAccess.UpdateMaps(id, updateMap);

    return Ok(map);  
}


    [HttpDelete("{id}")]
    public IActionResult DeleteMap(int id){
        var map = _mapDataAccess.GetMapById(id).Find(m => m.Id == id);
        if(map == null)
        {
            return NotFound();
        }

        _mapDataAccess.DeleteMaps(id);
        return NoContent();
    }

    [HttpGet("{id}/{x}-{y}")]
    public IActionResult CheckCoordinate(int id,int x, int y)
    {
        var map = _mapDataAccess.GetMapById(id).Find(m => m.Id == id);
        if(map == null){
            return BadRequest($"map {id} not found");
        }
        
        bool isWithinBounds = x <= map.Columns && x > 0 && y <= map.Rows && y > 0;
        if(isWithinBounds){
            return Ok(isWithinBounds);
        }

        
        else return NotFound("Place appropriate map coordinates");
    }
}
