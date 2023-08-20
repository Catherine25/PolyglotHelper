namespace PolyglotHelper.Database.Models;

public sealed class Activity : ActivityDbItem
{
    public Word Word { get; set; }
}

public class ActivityDbItem : DbItem
{
    public int WordId { get; set; }
    public bool Guessed { get; set; }
    public DateTime Date { get; set; }
}
