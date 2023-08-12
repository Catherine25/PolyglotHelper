using PolyglotHelper.Models;
using PolyglotHelper.Services;

namespace PolyglotHelper.Menu;

public partial class MainMenu : ContentView
{
    public event Action<Card> WordMetadataChanged;
    public event Action<Card> WordAnswered;

    private IStateService _stateService;
    private ILoggingService _loggingService;
    private Card _card;

    public MainMenu()
	{
		InitializeComponent();
        LeftPane.WordAnswered += LeftPane_WordAnswered;
        RightPane.CardChanged += RightPane_WordInContextChanged;
    }

    public void Init(IStateService stateService,
        ILoggingService loggingService)
    {
        _loggingService = loggingService;
        _stateService = stateService;
    }

    private async void LeftPane_WordAnswered(string answer)
    {
        _card.Word.RepeatTime = DateTime.Now;

        bool answeredCorrectly = Settings.IgnoreCase
            ? answer?.ToLower() == _card.Word.Word.ToLower()
            : answer == _card.Word.Word;

        var nextState = await _stateService.GetNextState(_card, answeredCorrectly);
        _card.Word.State = nextState;

        await _loggingService.LogActivity(_card, answeredCorrectly);
        
        if (!answeredCorrectly)
            LeftPane.SetInvalidAnsweredWordForRepeat(_card);
        else
        {
            LeftPane.SetWordAnsweredCorrectly();
            WordAnswered(_card);
        }    
    }

    private void RightPane_WordInContextChanged(Card card)
    {
        WordMetadataChanged(card);
    }

    public void SetCard(Card card)
    {
        _card = card;
        
        LeftPane.SetCard(_card);
        RightPane.SetCard(_card);
    }
}