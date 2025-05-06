// Views/UserPage.xaml.cs
using SaveUpAppFrontend.ViewModels;
namespace SaveUpAppFrontend.Views
{
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();

        }
        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            // Speichere das Sparziel oder andere Einstellungen
            if (BindingContext is UserViewModel viewModel)
            {
                viewModel.SaveSettings();
            }

            // Sende Nachricht, um SavingsPage zu aktualisieren
            MessagingCenter.Send(this, "UpdateSavingsPage");
        }
    }
}