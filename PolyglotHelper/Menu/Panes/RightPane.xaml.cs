using PolyglotHelper.Models;

namespace PolyglotHelper.Menu.Panes;

public partial class RightPane : ContentView
{
    public event Action<Card> CardChanged;

    private Card _card;

    public RightPane()
	{
		InitializeComponent();

        _saveWordNoteButton.Clicked += SaveWordNoteButton_Clicked;
        _saveSentenceNoteButton.Clicked += SaveSentenceNoteButton_Clicked;
	}

    private async void SaveSentenceNoteButton_Clicked(object sender, EventArgs e)
    {
        _card.Sentence.Note = _sentenceNoteEditor.Text;
        UpdateCard();
    }

    private async void SaveWordNoteButton_Clicked(object sender, EventArgs e)
    {
        _card.Word.Note = _wordNoteEntry.Text;
        UpdateCard();
    }

    public void SetCard(Card card)
    {
        _card = card;

        _wordNoteEntry.Text = _card.Word.Note;
        _sentenceNoteEditor.Text = _card.Sentence.Note;
        // _tagsView.SetTags(_card.Word.TagIds);
    }

    private async void UpdateCard()
    {
        MainPage.CardService.SetCurrentCard(_card);
        await MainPage.CardService.Sync();
        CardChanged(_card);
    }
}