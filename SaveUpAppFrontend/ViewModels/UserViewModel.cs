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

        private double _savingGoal;
        public double SavingGoal
        {
            get => _savingGoal;
            set
            {
                _savingGoal = value;
                Preferences.Set("SavingGoal", value); // Speichere das Sparziel
                OnPropertyChanged();
            }
        }

        private string _language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged();
            }
        }
        public async Task ReloadData() // Neue Methode zur Aktualisierung
        {
            await LoadData();
            OnPropertyChanged(nameof(TotalSavings));
            OnPropertyChanged(nameof(ProductCount));
        }
        public UserViewModel()
        {
            _apiService = new ApiService();
            InstallDate = Preferences.Get("InstallDate", DateTime.Now);
            Preferences.Set("InstallDate", InstallDate); // Wird beim ersten Start gesetzt
            SavingGoal = Preferences.Get("SavingGoal", 0.0); // Lade gespeichertes Ziel
            _ = LoadData();
        }
        public void SaveSettings()
        {
        Preferences.Set("SavingGoal", SavingGoal); // Speichere das Sparziel
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