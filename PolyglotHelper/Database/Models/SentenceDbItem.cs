namespace PolyglotHelper.Database.Models;

public class SentenceDbItem : DbItem
{
    public string Sentence { get; set; }
    public string Note { get; set; }
    public bool Blocked { get; set; }
}
