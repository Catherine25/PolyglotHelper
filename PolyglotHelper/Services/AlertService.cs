namespace PolyglotHelper.Services;

public static class AlertService
{
    public static void Init(MainPage mainPage)
    {
        _mainPage = mainPage;
    }

    private static MainPage _mainPage;

    public static async Task Alert(string title, string message, string cancel)
    {
        await _mainPage.DisplayAlert(title, message, cancel);
    }
}
