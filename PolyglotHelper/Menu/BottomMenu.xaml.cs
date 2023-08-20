using PolyglotHelper.Alerts;
using PolyglotHelper.Import;

namespace PolyglotHelper.Menu;

public partial class BottomMenu : ContentView
{
    public Action BlockWordRequest;
    public Action BlockSentenceRequest;

    public event Action NextWordRequest;
    public event Action ClipboardRequest;

    private IImporter _importer;

    public BottomMenu()
	{
		InitializeComponent();

        CopyButton.Clicked += CopyButton_Clicked;
        BlockWordButton.Clicked += BlockWordButton_Clicked;
        BlockSentenceButton.Clicked += BlockSentenceButton_Clicked;
        ImportButton.Clicked += ImportButton_Clicked;
    }

    private void BlockSentenceButton_Clicked(object sender, EventArgs e)
    {
        BlockWordRequest();
    }

    private void BlockWordButton_Clicked(object sender, EventArgs e)
    {
        BlockSentenceRequest();
    }

    private void CopyButton_Clicked(object sender, EventArgs e)
    {
        ClipboardRequest();
    }

    public void Init(IImporter importer)
    {
        _importer = importer;
    }

    private async void ImportButton_Clicked(object sender, EventArgs e)
    {
        var sentence = await MainPage.AlertService.ShowTextRequest();

        if (sentence == null)
            return;

        try
        {
            await _importer.Import(sentence);
            NextWordRequest();
        }
        catch (SentenceAlreadyImportedException)
        {
            await MainPage.AlertService.ShowSentenceAlreadyImported(sentence);
        }
    }
}