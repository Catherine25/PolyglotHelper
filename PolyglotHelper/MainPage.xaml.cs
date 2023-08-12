using PolyglotHelper.Import;
using PolyglotHelper.Models;
using PolyglotHelper.Services;

namespace PolyglotHelper;

public partial class MainPage : ContentPage
{
    private readonly ICardService _cardService;

    public MainPage(ICardService cardService,
        IStateService stateService,
        ILoggingService loggingService,
        IImporter importer)
	{
		InitializeComponent();

        AlertService.Init(this);

        BottomMenu.Init(importer);
        BottomMenu.TextRequest += TextRequest;
        BottomMenu.NextWordRequest += ShowNextWord;
        BottomMenu.ClipboardRequest += BottomMenu_ClipboardRequest;
        BottomMenu.BlockWordRequest += BottomMenu_BlockWordRequest;
        BottomMenu.BlockSentenceRequest += BottomMenu_BlockSentenceRequest;

        MainMenu.Init(stateService, loggingService);
        MainMenu.WordAnswered += MainMenu_WordAnswered;
        MainMenu.WordMetadataChanged += MainMenu_WordInContextChanged;

        _cardService = cardService;

        ShowNextWord();
    }

    private void BottomMenu_BlockSentenceRequest()
    {
        _cardService.BlockWord();
        ShowNextWord();
    }

    private void BottomMenu_BlockWordRequest()
    {
        _cardService.BlockSentence();
        ShowNextWord();
    }

    private async void BottomMenu_ClipboardRequest()
    {
        var card = await _cardService.GetCurrentCard();
        await Clipboard.SetTextAsync(card.Sentence.Sentence);
    }

    private void MainMenu_WordInContextChanged(Card card)
    {
        _cardService.SetCurrentCard(card);
        _cardService.Sync();

        ShowCardOnViews(card);
    }

    private void MainMenu_WordAnswered(Card card)
    {
        _cardService.SetCurrentCard(card);
        _cardService.Sync();

        ShowNextWord();
    }

    private async Task<string> TextRequest()
    {
        return await DisplayPromptAsync(Prompts.EnterText, Prompts.EnterText_Desc);
    }

    private async void ShowNextWord()
    {
        Card currentCard = await LoadNewCard();

        _cardService.SetCurrentCard(currentCard);

        if (currentCard != null)
            ShowCardOnViews(currentCard);

        //BottomMenuView.UpdateTodayStatistics(
        //    await wordService.GetWordsPracticedToday(),
        //    wordService.WordsAvailable);
    }

    private async Task<Card> LoadNewCard()
    {
        bool wordAvailable = await _cardService.AnyCardAvailable();

        if (!wordAvailable)
        {
            await DisplayAlert(Prompts.NoCardsAvailable, Prompts.NoCardsAvailable_Desc, Prompts.Ok);
            return null;
        }

        Card currentCard = await _cardService.GetCurrentCard();

        var readyForRepeating = await _cardService.GetCardsReadyForRepeating(currentCard.Sentence.Sentence);

        if (!readyForRepeating.Any())
        {
            await DisplayAlert(Prompts.NoCardsAvailable, Prompts.NoCardsAvailable_Desc, Prompts.Ok);
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

