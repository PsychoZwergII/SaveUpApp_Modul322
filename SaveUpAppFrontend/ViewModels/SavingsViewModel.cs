using SaveUpAppFrontend.Models;
using SaveUpAppFrontend.Services;
using System.Threading.Tasks;

namespace SaveUpAppFrontend.ViewModels
{
    public class SavingsViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private double _totalSavings;
        private double _savingGoal;

        public double TotalSavings
        {
            get => _totalSavings;
            set
            {
                _totalSavings = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProgressPercentage)); // Update progress
            }
        }

        public double SavingGoal
        {
            get => _savingGoal;
            set
            {
                _savingGoal = value > 0 ? value : 1.0; // Avoid division by zero
                Preferences.Set("SavingGoal", _savingGoal); // Save goal
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProgressPercentage)); // Update progress
            }
        }

        public double ProgressPercentage => SavingGoal > 0 ? Math.Min((TotalSavings / SavingGoal), 1.0) : 0;

        public SavingsViewModel()
        {
            _apiService = new ApiService();
            var savedValue = Preferences.Get("SavingGoal", "0"); // Hole den Wert als String
            if (int.TryParse(savedValue, out int savingGoal))
            {
                SavingGoal = savingGoal; // Konvertierung erfolgreich
            }
            else
            {
                SavingGoal = 0; // Fallback-Wert
            }
            _ = LoadTotalSavings();
        }

        public async Task ReloadData()
        {
            try
            {
                // Versuche, die Produkte von der API zu laden
                var products = await _apiService.GetProductsAsync();
                TotalSavings = products.Sum(p => p.Price);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API-Verbindung fehlgeschlagen: {ex.Message}. Lade Produkte aus der lokalen JSON-Datei...");

                // Fallback: Produkte aus der lokalen Datei laden
                var products = await _apiService.LoadFromLocalFileAsync();
                TotalSavings = products.Sum(p => p.Price);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein unerwarteter Fehler ist aufgetreten: {ex.Message}");
            }
            finally
            {
                // Benachrichtige die Benutzeroberfläche über Änderungen
                OnPropertyChanged(nameof(TotalSavings));
                OnPropertyChanged(nameof(ProgressPercentage));
            }
        }

        private async Task LoadTotalSavings()
        {
            try
            {
                // Versuche, die Produkte von der API zu laden
                var products = await _apiService.GetProductsAsync();
                TotalSavings = products.Sum(p => p.Price);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"--------- API-Verbindung fehlgeschlagen: {ex.Message}. Lade Produkte aus der lokalen JSON-Datei...");

                // Fallback: Lade Produkte aus der lokalen Datei
                var products = await _apiService.LoadFromLocalFileAsync();
                TotalSavings = products.Sum(p => p.Price);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--------- Ein unerwarteter Fehler ist aufgetreten: {ex.Message}");
            }
        }
    }
}