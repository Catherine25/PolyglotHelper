namespace PolyglotHelper.Database.Models;

public class TagDbItem : DbItem
{
    public TagDbItem()
    {
    }

    public TagDbItem(string tag)
    {
        Tag = tag;
    }

    public string Tag { get; set; }
}
