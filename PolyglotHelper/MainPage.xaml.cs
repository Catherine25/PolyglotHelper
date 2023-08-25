using PolyglotHelper.Alerts;
using PolyglotHelper.Database;
using PolyglotHelper.Import;
using PolyglotHelper.Models;
using PolyglotHelper.Services;

namespace PolyglotHelper;

public partial class MainPage : ContentPage
{
    public static AlertService AlertService { get; private set; }
    public static DatabaseService DatabaseService { get; private set; }
    public static ILoggingService LoggingService { get; private set; }
    public static TagService TagService { get; private set; }
    public static ICardService CardService { get; private set; }
    public static IImporter Importer { get; private set; }
    public static IStateService StateService { get; private set; }

    public MainPage()
	{
        AlertService = new AlertService(this);

        DatabaseService = new DatabaseService();
        LoggingService = new LoggingService(DatabaseService);
        TagService = new TagService(DatabaseService);
        Importer = new Importer(DatabaseService, new TextProcessor());
        StateService = new StateService(LoggingService, new NextGuessService(LoggingService));
        CardService = new CardService(DatabaseService, StateService, new CardDbService(DatabaseService));

        InitializeComponent();

        BottomMenu.NextWordRequest += ShowNextWord;
        MainMenu.NextWordRequest += ShowNextWord;
        MainMenu.RefreshViewsRequest += ShowCardOnViews;

         ShowNextWord();
    }

    private async void ShowNextWord()
    {
        Card currentCard = await LoadNewCard();

        CardService.SetCurrentCard(currentCard);

        if (currentCard != null)
            ShowCardOnViews(currentCard);

        //BottomMenuView.UpdateTodayStatistics(
        //    await wordService.GetWordsPracticedToday(),
        //    wordService.WordsAvailable);
    }

    private async Task<Card> LoadNewCard()
    {
        bool wordAvailable = await CardService.AnyCardAvailable();

        if (!wordAvailable)
        {
            await AlertService.ShowNoCardsAvailable();
            return null;
        }

        Card currentCard = await CardService.GetCurrentCard();

        var readyForRepeating = await CardService.GetCardsReadyForRepeating(currentCard.Sentence.Sentence);

        if (!readyForRepeating.Any())
        {
            await AlertService.ShowNoCardsAvailable();
            return null;
        }

        return readyForRepeating.First();
    }

    private void ShowCardOnViews(Card card)
    {
        TopMenu.SetCard(card);
        MainMenu.SetCard(card);

        this.UpdateChildrenLayout();
    }
}

