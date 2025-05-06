using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SaveUpAppFrontend.Models;
using SaveUpAppFrontend.Services;
using SaveUpAppFrontend.ViewModels;

namespace SaveUpAppFrontend.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public ObservableCollection<Product> Products { get; set; } = new();
        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DeleteAllCommand { get; }

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
            DeleteAllCommand = new Command(async () => await DeleteAllProducts());

            _ = LoadProducts();
        }

        /* private async Task LoadProducts()
         {
             try
             {
                 // Lade Produkte von der API
                 var products = await _apiService.GetProductsAsync();

                 // Leere die bestehende Liste (falls vorhanden)
                 //Products.Clear();

                 // Füge die Produkte zur ObservableCollection hinzu
                 foreach (var product in products)
                 {
                     Products.Add(product);
                 }

                 // Optional: Debugging-Log
                 Console.WriteLine($"Successfully loaded {Products.Count} products.");
             }
             catch (Exception ex)
             {
                 // Fehler behandeln
                 Console.WriteLine($"Error loading products: {ex.Message}");
             }
         }*/
        private async Task LoadProducts()
        {
            try
            {
                IsBusy = true;

                // Lade Produkte aus der Backend-API
                var products = await _apiService.GetProductsAsync();

                // Falls Backend offline, lade aus lokaler Datei
                if (!products.Any())
                {
                    products = await _apiService.LoadFromLocalFileAsync();
                }

                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
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
                {
                    await Application.Current.MainPage.DisplayAlert("Fehler", "Beschreibung und Preis müssen gültig sein!", "OK");
                    return;
                }

                NewProduct.Date = DateTime.Now;
                var created = await _apiService.AddProductAsync(NewProduct);

                if (created != null)
                {
                    Products.Add(created); // Aktualisiere die Liste
                    NewProduct = new Product(); // Zurücksetzen für neues Produkt
                    OnPropertyChanged(nameof(NewProduct));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Fehler", "Produkt konnte nicht hinzugefügt werden.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Hinzufügen des Produkts: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Fehler", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteAllProducts()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                // Lösche jedes Produkt einzeln
                foreach (var product in Products.ToList())
                {
                    await _apiService.DeleteProductAsync(product.Id); // API-Aufruf für jedes Produkt
                    Products.Remove(product); // Entferne das Produkt aus der Liste
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting all products: {ex.Message}");
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