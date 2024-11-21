using Microsoft.Extensions.Logging;
using Strafenkatalog.View;
using Strafenkatalog.ViewModel;

using Strafenkatalog.DataAccess;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;

namespace Strafenkatalog
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);

            builder.Services.AddDbContext<HandyDbContext>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<EditPlayerPage>();
            builder.Services.AddTransient<EditPlayerViewModel>();

            builder.Services.AddTransient<FinePage>();
            builder.Services.AddTransient<FineViewModel>();

            builder.Services.AddTransient<EditFinePage>();
            builder.Services.AddTransient<EditFineViewModel>();

            builder.Services.AddTransient<DetailPlayerPage>();
            builder.Services.AddTransient<DetailPlayerViewModel>();

            builder.Services.AddTransient<ArchivePlayerPage>();
            builder.Services.AddTransient<ArchivePlayerViewModel>();

            var dbContext = new HandyDbContext();
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
