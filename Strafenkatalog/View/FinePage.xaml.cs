using Strafenkatalog.ViewModel;

namespace Strafenkatalog.View;

public partial class FinePage : ContentPage
{
	public FinePage(FineViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}