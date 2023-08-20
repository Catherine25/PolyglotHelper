namespace PolyglotHelper.Database.Models;

public sealed class Word : WordDbItem
{
    public Sentence Sentence { get; set; }
}

public class WordDbItem : DbItem
{
    public string Word { get; set; }
    public int SentenceId { get; set; }
    public States State { get; set; }
    public DateTime? RepeatTime { get; set; }
    public string Note { get; set; }
    public bool Blocked { get; set; }
}
