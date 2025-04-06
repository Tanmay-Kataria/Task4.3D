using FULLAPI.Models;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

public class MapEF : IMapDataAccess
{
    private RobotContext _context;

    public MapEF(RobotContext context)
    {
        _context = context;
    }

    public List<Map> GetSquareMap()
    {
        return GetMaps(true, null);
    }

    public  List<Map> GetMapById(int id)
    {
        return GetMaps(null, id);
    } 

    public List<Map> GetMaps(bool? issquare, int? id)
    {
        var query = _context.Robotmaps.AsQueryable();

        if(id.HasValue)
        {
            query = _context.Robotmaps.Where(cmd => cmd.Id.Equals(id));
        }

        else if (issquare == true)
        {
            query = _context.Robotmaps.Where(cmd => cmd.IsSquare.Value == issquare.Value);
        }

        return query.ToList();
    }

    public void AddMaps(Map map)
    {
        map.CreatedDate = DateTime.Today;
        map.ModifiedDate = DateTime.Today;
        _context.Robotmaps.Add(map);
        _context.SaveChanges();
    }

    public void UpdateMaps(int id, Map map)
    {
        var existingmap = _context.Robotmaps.FirstOrDefault(map => map.Id.Equals(id));
        if (existingmap != null)
        {
            existingmap.Rows = map.Rows;
            existingmap.Columns = map.Columns;
            existingmap.Description = map.Description;
            existingmap.CreatedDate = map.CreatedDate;
            existingmap.ModifiedDate = map.ModifiedDate;

            _context.SaveChanges();
        }
    }

    public void DeleteMaps(int id)
    {
        var existingmap = _context.Robotmaps.Find(id);
        _context.Remove(existingmap);
        _context.SaveChanges();
    }

    
}