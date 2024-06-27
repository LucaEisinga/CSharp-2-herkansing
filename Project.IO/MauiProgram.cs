using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using Project.IO.Classes.Service;
using Project.IO.Utilities;

namespace Project.IO
{
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
                });
            builder.Services.AddSingleton<TaskService>();
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<SessionService>();
            builder.Services.AddScoped<ProjectAssignmentService>();
            builder.Services.AddScoped<ProjectUtil>();
            builder.Services.AddScoped<RoleService>();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddBlazorBootstrap();
            return builder.Build();
        }
    }
}
