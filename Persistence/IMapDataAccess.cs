using FULLAPI.Models;

namespace robot_controller_api.Persistence;
public interface IMapDataAccess {
    List<Map> GetMaps(bool? issquare, int? id);

    List<Map> GetSquareMap();
    List<Map> GetMapById(int id);
    void AddMaps(Map map);
    void UpdateMaps(int id, Map map);
    void DeleteMaps(int id);

}