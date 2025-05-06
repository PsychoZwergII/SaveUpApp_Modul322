using SaveUpAppFrontend.Services;
using SaveUpAppFrontend.ViewModels;

namespace SaveUpAppFrontend;

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
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
            });

        // Services und ViewModels registrieren
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<UserViewModel>();
        builder.Services.AddSingleton<SavingsViewModel>();
        builder.Services.AddSingleton<DashboardViewModel>();


        return builder.Build();
    }
}