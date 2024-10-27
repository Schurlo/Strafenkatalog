using Strafenkatalog.ViewModel;

namespace Strafenkatalog
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel viewModel;

        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            this.viewModel = vm;
            BindingContext = vm;
        }

        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    await viewModel.ListUpdate();
        //}
    }
}
