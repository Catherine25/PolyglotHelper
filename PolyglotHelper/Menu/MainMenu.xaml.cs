using PolyglotHelper.Models;

namespace PolyglotHelper.Menu;

public partial class MainMenu : ContentView
{
    public event Action<Card> RefreshViewsRequest;
    public event Action NextWordRequest;

    private Card _card;

    public MainMenu()
	{
        InitializeComponent();

        LeftPane.WordAnswered += LeftPane_WordAnswered;
        RightPane.CardChanged += RefreshViewsRequest;
    }

    private async void LeftPane_WordAnswered(string answer)
    {
        _card.Word.RepeatTime = DateTime.Now;

        bool answeredCorrectly = Settings.IgnoreCase
            ? answer?.ToLower() == _card.Word.Word.ToLower()
            : answer == _card.Word.Word;

        var nextState = await MainPage.StateService.GetNextState(_card, answeredCorrectly);
        _card.Word.State = nextState;

        await MainPage.LoggingService.LogActivity(_card, answeredCorrectly);
        
        if (!answeredCorrectly)
            LeftPane.SetInvalidAnsweredWordForRepeat(_card);
        else
        {
            LeftPane.SetWordAnsweredCorrectly();
            MainPage.CardService.SetCurrentCard(_card);
            await MainPage.CardService.Sync();
            NextWordRequest();
        }    
    }

    public void SetCard(Card card)
    {
        _card = card;
        
        LeftPane.SetCard(_card);
        RightPane.SetCard(_card);
    }
}