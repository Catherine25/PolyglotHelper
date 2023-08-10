using System.ComponentModel.DataAnnotations.Schema;

namespace PolyglotHelper.Database.Models;

public class WordDbItem : DbItem
{
    public string Word { get; set; }

    [ForeignKey(nameof(SentenceDbItem))]
    public int SentenceId { get; set; }
    public States State { get; set; }
    public DateTime? RepeatTime { get; set; }
    public string Note { get; set; }
    public bool Blocked { get; set; }
}
