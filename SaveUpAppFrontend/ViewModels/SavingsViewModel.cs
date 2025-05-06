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
            SavingGoal = Preferences.Get("SavingGoal", 0); // Set default saving goal
            _ = LoadTotalSavings();
        }

        public async Task ReloadData()
        {
            await LoadTotalSavings();
        }

        private async Task LoadTotalSavings()
        {
            try
            {
                var products = await _apiService.GetProductsAsync();
                TotalSavings = products.Sum(p => p.Price);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading savings: {ex.Message}");
            }
        }
    }
}