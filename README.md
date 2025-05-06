# SaveUpApp Modul322

Dieses Projekt ist eine Anwendung zum Verwalten und Verfolgen von Ersparnissen. Es besteht aus einer **Frontend**-Komponente (basierend auf .NET MAUI) und einer **Backend**-API (basierend auf ASP.NET Core). Dieses Dokument beschreibt, wie das Projekt lokal eingerichtet wird, welche Änderungen notwendig sind, welche Abhängigkeiten installiert werden müssen und wo sich die wichtigsten Dateien befinden.

---

## 🛠️ Lokale Änderungen und Einrichtung

### 1. **Frontend**
- **Ändern der API-URL**:
  - Öffne die Datei `ApiService.cs` im Ordner `SaveUpAppFrontend/Services/`.
  - Passe die `BaseAddress` an die lokale URL des Backends an:
    ```csharp
    _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7137/api/") // Lokale Backend-URL
    };
    ```

### 2. **Backend**
- **MongoDB-Verbindung einrichten**:
  - Öffne die Datei `appsettings.json` im Ordner `SaveUpAppBackend`.
  - Ändere die Verbindungsdetails für deine lokale MongoDB-Instanz:
    ```json
    "MongoDbSettings": {
      "ConnectionString": "mongodb://localhost:27017",
      "DatabaseName": "saveupapp",
      "ProductsCollectionName": "Products"
    }
    ```

---

## 📦 Zusätzliche Abhängigkeiten

### 1. **Backend**
- Installiere den MongoDB-Treiber für .NET:
  ```bash
  dotnet add package MongoDB.Driver
  ```
- Weitere Abhängigkeiten sind in der Datei `SaveUpAppBackend/SaveUpAppBackend.csproj` definiert.

### 2. **Frontend**
- Stelle sicher, dass die .NET MAUI-Umgebung korrekt eingerichtet ist.
  - [Offizielle Anleitung zur Einrichtung von .NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation)

---

## 📂 Projektstruktur und wichtige Dateien

### **Frontend**
- **`SaveUpAppFrontend/`**
  - **`Views/`**
    - **`SavingsPage.xaml` & `SavingsPage.xaml.cs`**: Zeigt den Sparfortschritt an und bindet das ViewModel.
    - **`DashboardPage.xaml` & `DashboardPage.xaml.cs`**: Übersicht über alle Produkte.
    - **`UserPage.xaml` & `UserPage.xaml.cs`**: Übersicht über Benutzerinfos und Einstellungen.
  - **`ViewModels/`**
    - **`SavingsViewModel.cs`**: Enthält die Logik zur Berechnung des Sparfortschritts und das Laden der Daten.
    - **`DashboardViewModel.cs`**: Logik für die Produktverwaltung (Hinzufügen, Löschen, Laden).
    - **`UserViewModel.cs`**: Speichern des Sparziels + infos zum Benutzer.
  - **`Services/`**
    - **`ApiService.cs`**: Schnittstelle zum Backend. Hier wird die Verbindung aufgebaut.
  - **`App.xaml` & `AppShell.xaml`**: Definieren die Navigation und globale Styles.

### **Backend**
- **`SaveUpAppBackend/`**
  - **`Controllers/`**
    - **`ProductsController.cs`**: API-Endpunkte zur Verwaltung von Produkten (CRUD).
  - **`Services/`**
    - **`MongoDBService.cs`**: Verbindet sich mit der MongoDB-Datenbank und führt Operationen aus.
  - **`Models/`**
    - **`Product.cs`**: Definiert das Datenmodell für Produkte.
  - **`appsettings.json`**: Konfigurationsdatei für die Datenbankverbindung.

---

## 🚀 Schnellstar in Terminal

### 1. **Backend starten**
- Navigiere in den Ordner `SaveUpAppBackend`:
  ```bash
  cd SaveUpAppBackend
  dotnet run
  ```
- Das Backend läuft standardmäßig auf `https://localhost:7137`.

### 2. **Frontend starten**
- Navigiere in den Ordner `SaveUpAppFrontend`:
  ```bash
  cd SaveUpAppFrontend
  dotnet build
  dotnet run
  ```
- Das Frontend wird gestartet und öffnet sich standardmäßig im Emulator oder auf einem Gerät.


## 🚀 Schnellstar in Visual Studio

### 1. Notwendige Einstellung
- Rechtsclick auf die Solution ->
  - Eigenschaften   ->
    - Startobjekte Konfigurieren ->
      - Mehrere Startobjekte anwählen ->
        - Zuletzt rechts bei Aktion Starten Wählen ->
          - Übernehmen und OK
          - F5 oder startknopf
---

## 🗂️ Wichtige Funktionen

### **Frontend**
1. **Sparfortschritt anzeigen und aktualisieren**:
   - Implementiert in `SavingsPage.xaml` und `SavingsViewModel.cs`.
   - Zeigt den prozentualen Fortschritt basierend auf der aktuellen Ersparnis und dem Sparziel an.
2. **Produkte verwalten**:
   - Implementiert in `DashboardPage.xaml` und `DashboardViewModel.cs`.
   - Ermöglicht das Hinzufügen, Löschen und Anzeigen von Produkten.

### **Backend**
1. **Produkte speichern und abrufen**:
   - Die API bietet Endpunkte zur Verwaltung von Produkten.
   - Implementiert in `ProductsController.cs` und `MongoDBService.cs`.

---

## 📋 To-Do für neue Entwickler

1. **Datenbankinitialisierung**:
   - Stelle sicher, dass eine MongoDB-Instanz läuft und die Konfiguration in `appsettings.json` korrekt ist.
2. **Tests durchführen**:
   - Teste, ob die Verbindung zwischen Frontend und Backend funktioniert.
3. **Neue Funktionen hinzufügen**:
   - Füge neue Seiten oder Endpunkte hinzu, falls erforderlich.

---
