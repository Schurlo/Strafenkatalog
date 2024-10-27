using Strafenkatalog.ViewModel;

namespace Strafenkatalog.View;

public partial class EditPlayerPage : ContentPage
{
    EditPlayerViewModel vm;

	public EditPlayerPage(EditPlayerViewModel vm)
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