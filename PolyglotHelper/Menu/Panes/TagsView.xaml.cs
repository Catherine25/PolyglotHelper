using PolyglotHelper.Database.Models;

namespace PolyglotHelper.Menu.Panes;

public partial class TagsView : ContentView
{
	private readonly Button _addButton;

    public TagsView()
    {
        InitializeComponent();

        _addButton = new Button() { Text = "+" };
        _addButton.Clicked += AddButton_Clicked;
    }

    public void SetTags(IEnumerable<TagDbItem> tags)
	{
		TagsLayout.Children.Clear();

        foreach (var tag in tags)
			BuildViewForTag(tag);

		TagsLayout.Children.Add(_addButton);
	}

	private void BuildViewForTag(TagDbItem tag)
	{
		TagsLayout.Children.Add(new Button() { Text = tag.Tag });
    }

    private async void AddButton_Clicked(object sender, EventArgs e)
    {
        var tags = await MainPage.TagService.GetAll();
        await MainPage.AlertService.ShowTagWizard();
        //TagsLayout.Children.Add(new Entry());
    }
}