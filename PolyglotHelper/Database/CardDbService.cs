using PolyglotHelper.Database.Models;
using PolyglotHelper.Models;

namespace PolyglotHelper.Database;

public interface ICardDbService
{
    Task AddCard(Card card);

    Task<IEnumerable<Card>> GetCards();

    Task SaveCard(Card card);
}

public class CardDbService : ICardDbService
{
    private DatabaseService _databaseService;
    
    public CardDbService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task AddCard(Card card)
    {
        var updatedSentence = await _databaseService.SaveSentenceAsync(card.Sentence);

        card.Word.SentenceId = updatedSentence.Id;

        await _databaseService.SaveWordAsync(card.Word);
    }

    public async Task<IEnumerable<Card>> GetCards()
    {
        var words = await _databaseService.GetItemsAsync<WordDbItem>();

        var sentences = await _databaseService.GetItemsAsync<SentenceDbItem>();

        var cards = words.Select(w =>
            new Card(w, sentences.Single(s => s.Id == w.SentenceId)));

        return cards;
    }

    public async Task SaveCard(Card card)
    {
        await _databaseService.SaveItemAsync(card.Word);
        await _databaseService.SaveItemAsync(card.Sentence);
    }
}
