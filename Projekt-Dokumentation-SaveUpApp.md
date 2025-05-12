## **1. Informieren**

### Ausgangslage
Die SaveUpApp_Modul322 wurde entwickelt, um Benutzern eine einfache Möglichkeit zu bieten, ihre Ersparnisse zu verwalten, Produkte zu organisieren und Sparziele zu verfolgen. Das Projekt besteht aus einer plattformübergreifenden **Frontend-App** (basierend auf .NET MAUI) und einem **Backend-Server** (basierend auf ASP.NET Core Web API), der als zentrale Datenablage dient.

### Ziele
- Erstellung einer plattformunabhängigen **Frontend-App**, die auf Android, iOS und Desktop läuft.
- Entwicklung eines robusten **Backend-Servers** zur Speicherung und Verwaltung der Daten.
- Offline-Funktionalität im Frontend, um Benutzern auch ohne Internetzugang Zugriff auf ihre Daten zu ermöglichen.
- Sicherstellung der Datenkonsistenz zwischen Frontend und Backend.

---

## **2. Planen**

### Mockups/Wireframes
Wireframes wurden vorab erstellt, um die Benutzeroberfläche zu planen und die Benutzererfahrung zu optimieren.

- **Dashboard**: Übersicht über alle Produkte und Gesamtersparnisse.
- **Produktseite**: Verwaltung von Produkten (Hinzufügen, Bearbeiten, Löschen).
- **Benutzerseite**: Einstellungen und Sparzielverwaltung.

### Gantt-Diagramm
Für die Planung wurde ein Gantt-Diagramm erstellt, das die einzelnen Arbeitspakete und deren Zeitrahmen visualisiert.

| **Arbeitspaket**                 | **Dauer**  | **Beschreibung**                              |
|----------------------------------|------------|----------------------------------------------|
| Backend-API erstellen            | 5 Tage     | Entwicklung der RESTful API und Datenbank.   |
| Frontend-Integration             | 5 Tage     | Integration der API ins Frontend.            |
| Offline-Funktionalität           | 3 Tage     | Implementierung der JSON-Dateispeicherung.   |
| Tests und Fehlerbehebung         | 3 Tage     | Durchführung von Tests und Debugging.        |

---

## **3. Entscheiden**

Nach der Analyse der Anforderungen wurde entschieden:
- **Frontend**: .NET MAUI wurde gewählt, um eine plattformübergreifende Entwicklung zu ermöglichen.
- **Backend**: ASP.NET Core Web API ermöglicht eine schnelle Entwicklung und einfache Integration mit MongoDB.
- **Datenhaltung**: MongoDB wurde aufgrund ihrer Flexibilität und Skalierbarkeit ausgewählt.

---

## **4. Realisieren**

### Technologie-Stack

#### Frontend
- **Technologie**: .NET MAUI
- **Architektur**: MVVM (Model-View-ViewModel)
- **Hauptkomponenten**:
  - Views: Benutzeroberfläche in XAML.
  - ViewModels: Logik und Datenbindung.
  - Services: API-Kommunikation und lokale Datenspeicherung.

#### Backend
- **Technologie**: ASP.NET Core Web API
- **Datenbank**: MongoDB
- **Hauptfunktionen**:
  - CRUD-Operationen für Produkte.
  - Unterstützung von JSON-Datenformaten.

### Projektstruktur

#### Frontend
```plaintext
SaveUpAppFrontend/
├── Views/
│   ├── DashboardPage.xaml
│   ├── SavingsPage.xaml
│   └── UserPage.xaml
├── ViewModels/
│   ├── DashboardViewModel.cs
│   ├── SavingsViewModel.cs
│   └── UserViewModel.cs
├── Services/
│   └── ApiService.cs
└── App.xaml
```

#### Backend
```plaintext
SaveUpAppBackend/
├── Controllers/
│   ├── ProductController.cs
├── Models/
│   ├── Product.cs
├── Services/
│   ├── MongoDBService.cs
├── appsettings.json
└── Program.cs
```

---

## **5. Kontrollieren**

### Testplan

#### Testfälle: Frontend
| **Testfall**                          | **Erwartetes Ergebnis**                       | **Status**   |
|---------------------------------------|-----------------------------------------------|--------------|
| Produkte von der API laden            | Produkte werden korrekt geladen.              | ✅ Bestanden |
| Produkte aus der JSON-Datei laden     | Produkte werden korrekt aus der Datei geladen.| ✅ Bestanden |
| Sparziel setzen und speichern         | Sparziel wird gespeichert und angezeigt.      | ✅ Bestanden |

#### Testfälle: Backend
| **Testfall**                          | **Erwartetes Ergebnis**                       | **Status**   |
|---------------------------------------|-----------------------------------------------|--------------|
| `GET /api/products`                   | Gibt alle Produkte zurück.                    | ✅ Bestanden |
| `POST /api/products`                  | Fügt ein neues Produkt hinzu.                 | ✅ Bestanden |
| `DELETE /api/products/{id}`           | Löscht ein Produkt erfolgreich.               | ✅ Bestanden |

### Testprotokoll
Alle Tests wurden erfolgreich durchgeführt. Kleinere Bugs, wie z. B. fehlerhafte UI-Aktualisierungen, wurden behoben.

---

## **6. Auswerten**

### Fazit
- Die Anwendung erfüllt alle definierten Anforderungen und bietet eine intuitive Benutzererfahrung.
- Die Offline-Funktionalität gewährleistet, dass Benutzer auch ohne Internetverbindung auf ihre Daten zugreifen können.
- Die Synchronisation zwischen Backend und Frontend funktioniert nahtlos.

### Reflexion
**Erfolge**:
- Klare Trennung von Logik und Benutzeroberfläche durch MVVM.
- Robuste Offline-Funktionalität.
- Stabile API-Integration.

**Verbesserungspotenziale**:
- Erweiterung der API um Benutzerauthentifizierung.
- Unterstützung für mehrere Sprachen im Frontend.

---

## **7. Quellen**
- [Microsoft MAUI Documentation](https://learn.microsoft.com/en-us/dotnet/maui/)
- [ASP.NET Core Web API Documentation](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
- [MongoDB Driver for .NET](https://www.mongodb.com/docs/drivers/csharp/)
