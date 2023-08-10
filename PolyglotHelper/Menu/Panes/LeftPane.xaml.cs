using PolyglotHelper.Extensions;
using PolyglotHelper.Models;

namespace PolyglotHelper.Menu.Panes;

public partial class LeftPane : ContentView
{
    public event Action<string> WordAnswered;

    private Entry _entry;

    public LeftPane()
	{
		InitializeComponent();
	}

    public void SetCard(Card card)
    {
        this.Log($"Showing word '{card.Word.Word}' in context '{card.Sentence.Sentence}'");
        this.Log($"Repeat time: '{card.Word.RepeatTime}'");

        var targetWordExtractor = new TargetWordExtractor();
        var sentenceParts = targetWordExtractor.ProduceWords(card.Sentence.Sentence, card.Word.Word);

        var shortened = SplitLongSentencesBy(sentenceParts, ',');
        var mappedWords = shortened
            .Select(w => (w, w == card.Word.Word ? card : null));

        Desk.Clear();

        GenerateWordViewsForWord(mappedWords);
    }

    public void SetInvalidAnsweredWordForRepeat(Card card)
    {
        _entry.Text = string.Empty;
        _entry.Placeholder = card.Word.Word;
    }

    public void SetWordAnsweredCorrectly()
    {
        var previousColor = _entry.TextColor;

        _entry.ColorTo(previousColor, Colors.Green, c => _entry.TextColor = c, 1000);
        Thread.Sleep(1000);
        _entry.ColorTo(Colors.Green, previousColor, c => _entry.TextColor = c, 1000);
        Thread.Sleep(1000);
    }

    private void GenerateWordViewsForWord(IEnumerable<(string contextWord, Card studiedWord)> mappedWords)
    {
        foreach (var item in mappedWords)
        {
            if (item.studiedWord == null)
            {
                Button label = new()
                {
                    Text = item.contextWord,
                    HeightRequest = 50,
                };

                Desk.Add(label);
            }
            else
            {
                _entry = new()
                {
                    HeightRequest = 50
                };

                _entry.Completed += (_, _) => View_Completed();
                Desk.Add(_entry);
            }
        }
    }

    private void View_Completed()
    {
        WordAnswered(_entry.Text);
    }

    private IEnumerable<string> SplitLongSentencesBy(IEnumerable<string> sentences, char character)
    {
        var result = sentences.SelectMany(s => new LongSentenceSplitter(s, 50).SplitSentenceBy(character));

        this.Log("SplitLongSentencesBy(): " + result.Count());

        return result;
    }
}