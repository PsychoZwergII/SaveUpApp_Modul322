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
        private readonly JsonStorageService<Product> _jsonStorage;

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
                    ((Command)AddCommand).ChangeCanExecute();
                    ((Command)DeleteCommand).ChangeCanExecute();
                }
            }
        }

        public DashboardViewModel()
        {
            _apiService = new ApiService();
            _jsonStorage = new JsonStorageService<Product>("products.json");

            LoadCommand = new Command(async () => await LoadProducts());
            AddCommand = new Command(async () => await AddProduct(), () => !IsBusy);
            DeleteCommand = new Command<int>(async (id) => await DeleteProduct(id), (id) => !IsBusy);
            DeleteAllCommand = new Command(async () => await DeleteAllProducts());

            _ = LoadProducts();
        }

        private async Task LoadProducts()
        {
            try
            {
                IsBusy = true;

                // Versuche, Produkte aus der API zu laden
                var products = new List<Product>();
                try
                {
                    products = await _apiService.GetProductsAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"API nicht verfügbar, lade Produkte aus der lokalen Datei: {ex.Message}");
                }

                // Falls keine Produkte geladen wurden, verwende die lokale JSON-Datei
                if (!products.Any())
                {
                    Console.WriteLine("Lade Produkte aus der lokalen JSON-Datei...");
                    products = await _jsonStorage.LoadFromFileAsync();
                }

                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }

                // Synchronisiere die JSON-Datei mit der API, wenn Daten existieren
                if (products.Any())
                {
                    await _jsonStorage.SaveToFileAsync(products);
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

                Product created = null;
                try
                {
                    // Versuche, das Produkt über die API hinzuzufügen
                    created = await _apiService.AddProductAsync(NewProduct);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"API nicht verfügbar, speichere Produkt lokal: {ex.Message}");
                }

                if (created != null)
                {
                    Products.Add(created);
                    // Synchronisiere die JSON-Datei
                    await _jsonStorage.SaveToFileAsync(Products.ToList());
                    Console.WriteLine("Produkt wurde sowohl in der API als auch in der JSON-Datei hinzugefügt.");
                }
                else
                {
                    // Füge das Produkt zur lokalen JSON-Datei hinzu
                    NewProduct.Id = Products.Any() ? Products.Max(p => p.Id) + 1 : 1;
                    Products.Add(NewProduct);
                    await _jsonStorage.SaveToFileAsync(Products.ToList());
                    Console.WriteLine("Produkt wurde zur JSON-Datei hinzugefügt.");
                }

                NewProduct = new Product();
                OnPropertyChanged(nameof(NewProduct));

                // Produkte neu laden, um die Benutzeroberfläche zu aktualisieren
                await ReloadProducts();
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
        private async Task ReloadProducts()
        {
            Products.Clear();
            var products = await _jsonStorage.LoadFromFileAsync();
            foreach (var product in products)
            {
                Products.Add(product);
            }
            Console.WriteLine("Produkte aus der JSON-Datei neu geladen.");
        }

        private async Task DeleteProduct(int id)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                try
                {
                    // Versuche, das Produkt über die API zu löschen
                    await _apiService.DeleteProductAsync(id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"API nicht verfügbar, lösche Produkt lokal: {ex.Message}");
                }

                var item = Products.FirstOrDefault(p => p.Id == id);
                if (item != null)
                {
                    Products.Remove(item);
                    await _jsonStorage.SaveToFileAsync(Products.ToList());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
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

                foreach (var product in Products.ToList())
                {
                    try
                    {
                        // Versuche, das Produkt über die API zu löschen
                        await _apiService.DeleteProductAsync(product.Id);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"API nicht verfügbar, lösche Produkt lokal: {ex.Message}");
                    }

                    Products.Remove(product);
                }

                // Aktualisiere die lokale JSON-Datei
                await _jsonStorage.SaveToFileAsync(Products.ToList());
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
    }
}