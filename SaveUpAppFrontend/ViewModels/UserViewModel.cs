// ViewModels/UserViewModel.cs
using SaveUpAppFrontend.Models;
using SaveUpAppFrontend.Services;
using System.Globalization;

namespace SaveUpAppFrontend.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public DateTime InstallDate { get; set; }
        public double TotalSavings { get; set; }
        public int ProductCount { get; set; }

        private string _language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged();
                // Spracheinstellung könnte hier lokal gespeichert oder angewendet werden
            }
        }

        public UserViewModel()
        {
            _apiService = new ApiService();
            InstallDate = Preferences.Get("InstallDate", DateTime.Now);
            Preferences.Set("InstallDate", InstallDate); // Wird beim ersten Start gesetzt
            _ = LoadData();
        }

        private async Task LoadData()
        {
            var products = await _apiService.GetProductsAsync();
            ProductCount = products.Count;
            TotalSavings = products.Sum(p => p.Price);
            OnPropertyChanged(nameof(ProductCount));
            OnPropertyChanged(nameof(TotalSavings));
        }
    }
}