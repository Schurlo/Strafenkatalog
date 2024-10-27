using Strafenkatalog.ViewModel;

namespace Strafenkatalog.View;

public partial class EditFinePage : ContentPage
{
	EditFineViewModel vm;

	public EditFinePage(EditFineViewModel vm)
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