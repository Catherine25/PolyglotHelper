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

    private void SaveSentenceNoteButton_Clicked(object sender, EventArgs e)
    {
        _card.Sentence.Note = _sentenceNoteEditor.Text;
        
        CardChanged(_card);
    }

    private void SaveWordNoteButton_Clicked(object sender, EventArgs e)
    {
        _card.Word.Note = _wordNoteEntry.Text;

        CardChanged(_card);
    }

    public void SetCard(Card card)
    {
        _card = card;

        _wordNoteEntry.Text = _card.Word.Note;
        _sentenceNoteEditor.Text = _card.Sentence.Note;
    }
}