namespace PolyglotHelper.Services;

public static class AlertService
{
    public static async Task ShowSentenceAlreadyImported(string sentence)
    {
        await BaseAlertService.DisplayAlert("The sentence has been already imported!", $"The sentence '{sentence}' has been already imported!", "Ok");
    }

    public static async Task<string> ShowTextRequest()
    {
        return await BaseAlertService.DisplayPromptAsync("Enter text", "Please enter text.");
    }

    public static async Task ShowNoCardsAvailable()
    {
        await BaseAlertService.DisplayAlert("No cards available!", "Please import more for practicing.", BaseAlertService.Ok);
    }
    
    public static async Task ShowTagWizard()
    {
        const string CreateNew = "Create new";

        var result = await BaseAlertService.DisplayActionSheet("Choose tag", CreateNew, BaseAlertService.Cancel);

        if (result == BaseAlertService.Cancel)
            return;

        if (result == CreateNew)
            throw new NotImplementedException();

        throw new NotImplementedException();
    }
}

public static class BaseAlertService
{
    public const string Yes = "Yes";
    public const string No = "No";
    public const string Ok = "Ok";
    public const string Cancel = "Cancel";

    public static void Init(MainPage mainPage)
    {
        _mainPage = mainPage;
    }

    private static MainPage _mainPage;

    public static async Task DisplayAlert(string title, string description, string cancel)
    {
        await _mainPage.DisplayAlert(title, description, cancel);
    }

    public static async Task<string> DisplayActionSheet(string title, string confirm, string cancel, params string[] options)
    {
        return await _mainPage.DisplayActionSheet(title, confirm, cancel, options);
    }

    public static async Task<string> DisplayPromptAsync(string title, string description)
    {
        return await _mainPage.DisplayPromptAsync(title, description);
    }
}
