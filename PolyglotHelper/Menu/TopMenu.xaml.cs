using PolyglotHelper.Models;

namespace PolyglotHelper.Menu;

public partial class TopMenu : ContentView
{
	private Card _wordInContext;

    public TopMenu()
	{
		InitializeComponent();
	}

	public void SetCard(Card wordInContext)
	{
		_wordInContext = wordInContext;

		StateLabel.Text = _wordInContext.Word.State.ToString();
    }
}