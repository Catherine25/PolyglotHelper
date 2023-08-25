using PolyglotHelper.Database;
using PolyglotHelper.Extensions;
using PolyglotHelper.Models;

namespace PolyglotHelper.Services;

public interface ICardService
{
    Task<Card> GetCurrentCard();
    void SetCurrentCard(Card wordInContext);

    Task Sync();

    Task<bool> AnyCardAvailable();
    Task<IEnumerable<Card>> GetCardsReadyForRepeating(string currentContext);

    Task BlockWord();
    Task BlockSentence();
}

public class CardService : ICardService
{
    private readonly DatabaseService _databaseService;
    private readonly IStateService _stateService;
    private readonly ICardDbService _cardDbService;
    private Card _currentCard;

    public CardService(
        DatabaseService databaseService,
        IStateService stateService,
        ICardDbService cardDbService)
    {
        _databaseService = databaseService;
        _stateService = stateService;
        _cardDbService = cardDbService;
    }

    public async Task<Card> GetCurrentCard()
    {
        var awailableCards = await GetCardsReadyForRepeating();
        return _currentCard ?? awailableCards.First();
    }

    public void SetCurrentCard(Card card)
    {
        _currentCard = card;
    }

    public async Task Sync()
    {
        await _databaseService.SaveItemAsync(_currentCard.Sentence);
        await _databaseService.SaveItemAsync(_currentCard.Word);
    }

    public async Task<bool> AnyCardAvailable()
    {
        var awailableCards = await GetCardsReadyForRepeating();
        return awailableCards.Any();
    }

    public async Task<IEnumerable<Card>> GetCardsReadyForRepeating(string currentContext = null)
    {
        var cards = await _cardDbService.GetCards();

        var cardsNotBlocked = cards.Where(c => !c.Word.Blocked);
        this.Log($"{cardsNotBlocked.Count()} words are not blocked");

        // word can be studied
        List<Card> cardsToStudy = new();
        foreach (var item in cardsNotBlocked)
        {
            bool readyToForRepeating = await _stateService.ReadyToForRepeating(item);

            if (readyToForRepeating)
                cardsToStudy.Add(item);
        }
        //var cardsToStudy = cardsNotBlocked.Where(c => _stateService.ReadyToForRepeating(c).Result); 
        this.Log($"{cardsToStudy.Count} words are ready for repeating");

        var mostKnownCards = cardsToStudy.OrderByDescending(c => c.Word.State); // first repeat most known

        return mostKnownCards
            .Where(c => c.Sentence.Sentence != currentContext) // show words with different sentence from the previous one to not spoil the word to the user
            .ToList();
    }

    public async Task BlockWord()
    {
        _currentCard.Word.Blocked = true;
        await Sync();
    }

    public async Task BlockSentence()
    {
        _currentCard.Sentence.Blocked = true;
        await Sync();
    }
}
