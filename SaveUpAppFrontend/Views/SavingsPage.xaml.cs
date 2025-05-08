using SaveUpAppFrontend.ViewModels;

namespace SaveUpAppFrontend.Views
{
    public partial class SavingsPage : ContentPage
    {
        public SavingsPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler in InitializeComponent: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"InnerException: {ex.InnerException.Message}");
            }

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
