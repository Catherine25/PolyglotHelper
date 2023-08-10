using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PolyglotHelper.Database;
using PolyglotHelper.Import;
using PolyglotHelper.Menu;
using PolyglotHelper.Menu.Panes;
using PolyglotHelper.Services;

namespace PolyglotHelper;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.UseMauiCommunityToolkit();

		builder.Services.AddSingleton<ICardService, CardService>();
		builder.Services.AddSingleton<IStateService, StateService>();
		builder.Services.AddSingleton<ITextProcessor, TextProcessor>();
		builder.Services.AddSingleton<ILoggingService, LoggingService>();
		builder.Services.AddSingleton<INextGuessService, NextGuessService>();
        builder.Services.AddSingleton<DatabaseService>();

		builder.Services.AddSingleton<IImporter, Importer>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<BottomMenu>();
        builder.Services.AddTransient<LeftPane>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
