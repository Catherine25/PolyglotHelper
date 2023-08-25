using PolyglotHelper.Import;

namespace PolyglotHelper.Menu;

public partial class BottomMenu : ContentView
{
    public event Action NextWordRequest;
    public event Action ClipboardRequest;

    public BottomMenu()
	{
        InitializeComponent();

        CopyButton.Clicked += CopyButton_Clicked;
        BlockWordButton.Clicked += BlockWordButton_Clicked;
        BlockSentenceButton.Clicked += BlockSentenceButton_Clicked;
        ImportButton.Clicked += ImportButton_Clicked;
    }

    private async void BlockSentenceButton_Clicked(object sender, EventArgs e)
    {
        await MainPage.CardService.BlockSentence();
        NextWordRequest();
    }

    private async void BlockWordButton_Clicked(object sender, EventArgs e)
    {
        await MainPage.CardService.BlockWord();
        NextWordRequest();
    }

    private async void CopyButton_Clicked(object sender, EventArgs e)
    {
        var card = await MainPage.CardService.GetCurrentCard();
        await Clipboard.SetTextAsync(card.Sentence.Sentence);
    }

    private async void ImportButton_Clicked(object sender, EventArgs e)
    {
        var sentence = await MainPage.AlertService.ShowTextRequest();

        if (sentence == null)
            return;

        try
        {
            await MainPage.Importer.Import(sentence);
            NextWordRequest();
        }
        catch (SentenceAlreadyImportedException)
        {
            await MainPage.AlertService.ShowSentenceAlreadyImported(sentence);
        }
    }
}