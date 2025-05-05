using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SaveUpAppFrontend.Models;
using SaveUpAppFrontend.Services;

namespace SaveUpAppFrontend.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public ObservableCollection<Product> Products { get; set; } = new();
        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public Product NewProduct { get; set; } = new();

        private string _newDescription = string.Empty;
        private double _newPrice;
        private bool _isBusy;

        public string NewDescription
        {
            get => _newDescription;
            set
            {
                if (_newDescription != value)
                {
                    _newDescription = value;
                    OnPropertyChanged();
                }
            }
        }

        public double NewPrice
        {
            get => _newPrice;
            set
            {
                // Wenn der Wert geändert wird, auf zwei Dezimalstellen runden
                if (_newPrice != value)
                {
                    _newPrice = Math.Round(value, 2);
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                    // Optionale Anpassung: Buttons deaktivieren, wenn IsBusy true ist
                    ((Command)AddCommand).ChangeCanExecute();
                    ((Command)DeleteCommand).ChangeCanExecute();
                }
            }
        }

        public DashboardViewModel()
        {
            _apiService = new ApiService();
            LoadCommand = new Command(async () => await LoadProducts());
            AddCommand = new Command(async () => await AddProduct(), () => !IsBusy);
            DeleteCommand = new Command<int>(async (id) => await DeleteProduct(id), (id) => !IsBusy);

            _ = LoadProducts();
        }

        private async Task LoadProducts()
        {
            try
            {
                IsBusy = true;
                var products = await _apiService.GetProductsAsync();
                Products.Clear();
                foreach (var p in products)
                    Products.Add(p);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., API errors)
                Console.WriteLine($"Error loading products: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddProduct()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                if (string.IsNullOrWhiteSpace(NewProduct.Description) || NewProduct.Price <= 0)
                    return;

                NewProduct.Date = DateTime.Now;
                var created = await _apiService.AddProductAsync(NewProduct);
                Products.Add(created);

                // Eingabefelder zurücksetzen
                NewProduct = new Product(); // neues leeres Produkt
                OnPropertyChanged(nameof(NewProduct));
            }
            catch (Exception ex)
            {
                // Detaillierte Fehlerausgabe
                Console.WriteLine($"Fehler beim Hinzufügen des Produkts: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Fehler", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        private async Task DeleteProduct(int id)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await _apiService.DeleteProductAsync(id);
                var item = Products.FirstOrDefault(p => p.Id == id);
                if (item != null)
                    Products.Remove(item);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., API errors)
                Console.WriteLine($"Error deleting product: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}