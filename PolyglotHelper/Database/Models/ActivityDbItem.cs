namespace PolyglotHelper.Database.Models;

public class ActivityDbItem : DbItem
{
    public int WordId { get; set; }
    public bool Guessed { get; set; }
    public DateTime Date { get; set; }
}
