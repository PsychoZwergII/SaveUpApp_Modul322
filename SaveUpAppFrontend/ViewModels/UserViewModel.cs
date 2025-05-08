using SaveUpAppFrontend.Models;
using SaveUpAppFrontend.Services;
using System.Globalization;

namespace SaveUpAppFrontend.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public Command ReloadCommand { get; }
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
        public async Task ReloadData()
        {
            try
            {
                // Versuche, die Produkte von der API zu laden
                var products = await _apiService.GetProductsAsync();
                ProductCount = products.Count;
                TotalSavings = products.Sum(p => p.Price);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API-Verbindung fehlgeschlagen: {ex.Message}. Lade Produkte aus der lokalen JSON-Datei...");

                // Fallback: Produkte aus der lokalen Datei laden
                var products = await _apiService.LoadFromLocalFileAsync();
                ProductCount = products.Count;
                TotalSavings = products.Sum(p => p.Price);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein unerwarteter Fehler ist aufgetreten: {ex.Message}");
                ProductCount = 0;
                TotalSavings = 0;
            }
            finally
            {
                // Benachrichtige die Benutzeroberfläche über Änderungen
                OnPropertyChanged(nameof(ProductCount));
                OnPropertyChanged(nameof(TotalSavings));
            }
        }
        public UserViewModel()
        {
            _apiService = new ApiService();
            InstallDate = Preferences.Get("InstallDate", DateTime.Now);
            Preferences.Set("InstallDate", InstallDate); // Wird beim ersten Start gesetzt
            SavingGoal = Preferences.Get("SavingGoal", 0.0); // Lade gespeichertes Ziel
            ReloadCommand = new Command(async () => await ReloadData());
            _ = LoadData();
        }
        public void SaveSettings()
        {
            Preferences.Set("SavingGoal", SavingGoal); // Speichere das Sparziel
            
        }
        private async Task LoadData()
        {
            try
            {
                // Versuche, die Produkte von der API zu laden
                
                var products = await _apiService.GetProductsAsync();
                ProductCount = products.Count;
                TotalSavings = products.Sum(p => p.Price);
                ReloadData();
                
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API-Verbindung fehlgeschlagen: {ex.Message}. Lade Produkte aus der lokalen JSON-Datei...");

                // Fallback: Lade Produkte aus der lokalen Datei
                
                var products = await _apiService.LoadFromLocalFileAsync();
                ProductCount = products.Count;
                TotalSavings = products.Sum(p => p.Price);
                ReloadData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein unerwarteter Fehler ist aufgetreten: {ex.Message}");
            }
            finally
            {
                // Benachrichtige die UI über Änderungen
                OnPropertyChanged(nameof(ProductCount));
                OnPropertyChanged(nameof(TotalSavings));
            }
        }
    }
}