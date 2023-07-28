using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MoviesTvApp.Pages;


namespace MoviesTvApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .UseMauiCommunityToolkitMarkup()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});


		builder.Logging.AddDebug();

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddScoped<VideoPage>();

		


        return builder.Build();
	}

    


}
