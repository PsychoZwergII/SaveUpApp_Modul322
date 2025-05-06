using SaveUpAppFrontend.ViewModels;

namespace SaveUpAppFrontend.Views
{
    public partial class SavingsPage : ContentPage
    {
        public SavingsPage()
        {
            InitializeComponent();
            BindingContext = new SavingsViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is SavingsViewModel viewModel)
            {
                await viewModel.ReloadData();
            }
        }
    }
}
