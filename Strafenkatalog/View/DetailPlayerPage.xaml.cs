using Strafenkatalog.ViewModel;

namespace Strafenkatalog.View;

public partial class DetailPlayerPage : ContentPage
{
    DetailPlayerViewModel vm;

	public DetailPlayerPage(DetailPlayerViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        this.vm = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        vm.Check();
    }
}