// ViewModels/SavingsViewModel.cs
using SaveUpAppFrontend.Models;
using SaveUpAppFrontend.ViewModels;
using SaveUpAppFrontend.Services;
using System.Collections.ObjectModel;

namespace SaveUpAppFrontend.ViewModels
{
    public class SavingsViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private double _totalSavings;

        public double TotalSavings
        {
            get => _totalSavings;
            set
            {
                _totalSavings = value;
                OnPropertyChanged();
            }
        }

        public SavingsViewModel()
        {
            _apiService = new ApiService();
            _ = LoadTotalSavings();
        }

        private async Task LoadTotalSavings()
        {
            var products = await _apiService.GetProductsAsync();
            TotalSavings = products.Sum(p => p.Price);
        }
    }
}
