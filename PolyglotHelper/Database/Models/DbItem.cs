using SQLite;

namespace PolyglotHelper.Database.Models;

public class DbItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
}
