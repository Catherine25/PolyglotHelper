using PolyglotHelper.Database.Models;
using PolyglotHelper.Services;

namespace PolyglotHelper.Alerts;

public static class AlertService
{
    public static async Task ShowSentenceAlreadyImported(string sentence)
    {
        await BaseAlertService.DisplayAlert(
            "The sentence has been already imported!",
            $"The sentence '{sentence}' has been already imported!",
            Responses.Ok);
    }

    public static async Task<string> ShowTextRequest()
    {
        return await BaseAlertService.DisplayPromptAsync(
            "Enter text",
            "Please enter text.");
    }

    public static async Task ShowNoCardsAvailable()
    {
        await BaseAlertService.DisplayAlert(
            "No cards available!",
            "Please import more for practicing.",
            Responses.Ok);
    }

    public static async Task ShowTagWizard()
    {
        const string createNew = "Create new";
        var tags = await TagService.GetAll();
        var tagStrings = tags.Select(o => o.Tag);

        var result = await BaseAlertService.DisplayActionSheet("Choose tag", createNew, Responses.Cancel, tagStrings.ToArray());

        if (result == Responses.Cancel)
            return;

        if (result == createNew)
        {
            string newTag = await BaseAlertService.DisplayPromptAsync(
                "Create tag",
                "Please enter new tag.");

            if (tagStrings.Contains(newTag))
            {
                await BaseAlertService.DisplayAlert(
                    "Error creating tag",
                    "The tag already exists!",
                    Responses.Ok);
            }
            else
                TagService.Add(new TagDbItem(newTag));
        }

        throw new NotImplementedException();
        System.Diagnostics.Debug.Assert(result == null || result == string.Empty);
    }
}
