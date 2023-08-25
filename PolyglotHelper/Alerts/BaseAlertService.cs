namespace PolyglotHelper.Alerts;

public class BaseAlertService
{
    private readonly MainPage _mainPage;

    public BaseAlertService(MainPage mainPage)
    {
        _mainPage = mainPage;
    }

    public async Task DisplayAlert(string title, string description, string cancel)
    {
        await _mainPage.DisplayAlert(title, description, cancel);
    }

    public async Task<string> DisplayActionSheet(string title, string confirm, string cancel)
    {
        return await _mainPage.DisplayActionSheet(title, confirm, cancel);
    }

    public async Task<string> DisplayActionSheet(string title, string confirm, string cancel, params string[] options)
    {
        return await _mainPage.DisplayActionSheet(title, confirm, cancel, options);
    }

    public async Task<string> DisplayPromptAsync(string title, string description)
    {
        return await _mainPage.DisplayPromptAsync(title, description);
    }
}
