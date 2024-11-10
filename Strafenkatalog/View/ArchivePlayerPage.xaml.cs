using Strafenkatalog.ViewModel;

namespace Strafenkatalog.View;

public partial class ArchivePlayerPage : ContentPage
{
    ArchivePlayerViewModel vm;

	public ArchivePlayerPage(ArchivePlayerViewModel vm)
	{
		InitializeComponent();
        this.vm = vm;
		BindingContext = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        vm.Check();
    }
}