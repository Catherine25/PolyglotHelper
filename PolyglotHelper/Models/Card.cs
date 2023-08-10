using PolyglotHelper.Database.Models;

namespace PolyglotHelper.Models;

public record Card(WordDbItem Word, SentenceDbItem Sentence, IEnumerable<TagDbItem> Tags)
{
}
