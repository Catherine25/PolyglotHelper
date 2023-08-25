using PolyglotHelper.Database.Models;

namespace PolyglotHelper.Alerts;

public class AlertService : BaseAlertService
{
    public AlertService(MainPage mainPage) : base(mainPage)
    {
    }

    public async Task ShowSentenceAlreadyImported(string sentence)
    {
        await DisplayAlert(
            "The sentence has been already imported!",
            $"The sentence '{sentence}' has been already imported!",
            Responses.Ok);
    }

    public async Task<string> ShowTextRequest()
    {
        return await DisplayPromptAsync(
            "Enter text",
            "Please enter text.");
    }

    public async Task ShowNoCardsAvailable()
    {
        await DisplayAlert(
            "No cards available!",
            "Please import more for practicing.",
            Responses.Ok);
    }

    public async Task ShowTagWizard()
    {
        const string createNew = "Create new";
        var tags = await MainPage.TagService.GetAll();
        var tagStrings = tags.Select(o => o.Tag);

        var result = await DisplayActionSheet("Choose tag", createNew, Responses.Cancel, tagStrings.ToArray());

        if (result == Responses.Cancel)
            return;

        if (result == createNew)
        {
            string newTag = await DisplayPromptAsync(
                "Create tag",
                "Please enter new tag.");

            if (tagStrings.Contains(newTag))
            {
                await DisplayAlert(
                    "Error creating tag",
                    "The tag already exists!",
                    Responses.Ok);
            }
            else
                MainPage.TagService.Add(new TagDbItem(newTag));
        }

        throw new NotImplementedException();
        System.Diagnostics.Debug.Assert(result == null || result == string.Empty);
    }
}
