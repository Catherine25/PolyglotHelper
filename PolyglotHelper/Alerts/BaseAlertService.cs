namespace PolyglotHelper.Alerts;

public static class BaseAlertService
{
    public static void Init(MainPage mainPage)
    {
        _mainPage = mainPage;
    }

    private static MainPage _mainPage;

    public static async Task DisplayAlert(string title, string description, string cancel)
    {
        await _mainPage.DisplayAlert(title, description, cancel);
    }

    public static async Task<string> DisplayActionSheet(string title, string confirm, string cancel)
    {
        return await _mainPage.DisplayActionSheet(title, confirm, cancel);
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
