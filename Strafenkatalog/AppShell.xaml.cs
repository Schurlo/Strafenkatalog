using Strafenkatalog.View;

namespace Strafenkatalog
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(EditPlayerPage), typeof(EditPlayerPage));

            Routing.RegisterRoute(nameof(EditFinePage), typeof(EditFinePage));

            Routing.RegisterRoute(nameof(DetailPlayerPage), typeof(DetailPlayerPage));
        }
    }
}
