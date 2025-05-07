using System.Text.Json;
using SaveUpAppFrontend.Models;

namespace SaveUpAppFrontend.Services
{
    public class JsonStorageService<T>
    {
        private readonly string _filePath;

        public JsonStorageService(string fileName)
        {
            // Speicherpfad im Projektverzeichnis
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        // Speichert die Daten in die JSON-Datei
        public async Task SaveToFileAsync(IEnumerable<T> items)
        {
            try
            {
                var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern in die Datei: {ex.Message}");
            }
        }

        // Lädt die Daten aus der JSON-Datei
        public async Task<List<T>> LoadFromFileAsync()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = await File.ReadAllTextAsync(_filePath);
                    return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der Datei: {ex.Message}");
            }

            return new List<T>();
        }
    }
}