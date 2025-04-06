using System.ComponentModel.DataAnnotations;

namespace FULLAPI.Models;

public class Map{
    [Key]
    public int Id {get; set;}
    public int Columns {get; set;}
    public int Rows {get; set;}
    public string? Description { get; set; }
    public DateTime CreatedDate {get; set;}
    public DateTime ModifiedDate {get; set;}

    public bool? IsSquare { get; set; }

    public Map() { }

    public Map (int id, int columns, int rows, DateTime createdDate, DateTime modifiedDate, string? description = null){
        Id = id;
        Columns = columns;
        Rows = rows;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
        Description = description;
    }
}