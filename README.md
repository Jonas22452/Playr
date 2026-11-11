# Playr

Playr is een Blazor-app die populaire spellen uit een lokale Steam-CSV toont. De app heeft een aanbevelingslaag, een zoekbaar spellenoverzicht en een detailvenster. De Library-pagina is voorlopig een placeholder.

## Starten met Docker

Benodigd: Git, Docker Desktop (met Docker Compose) en de lokale dataset `games.csv`. Docker Desktop moet draaien. Node en .NET hoeven niet lokaal geïnstalleerd te zijn.

```powershell
git clone https://github.com/Jonas22452/Playr.git
cd Playr
```

Zet je eigen `games.csv` in `Data/games.csv`. Bij deze opzet blijft de data op je computer staan. De CSV wordt niet gecommit, in de image gekopieerd of naar GitHub Actions geüpload. Heb je de dataset al in deze map, dan hoef je niets te verplaatsen.

Start daarna de volledige app met één commando:

```powershell
docker compose up --build
```

Open <http://localhost:8080>. Stop met `Ctrl+C`. Met `docker compose down` verwijder je de container; je lokale CSV blijft staan. Bij een ontbrekende CSV stopt de app met een duidelijke foutmelding.

Op Linux of macOS zijn dezelfde Git- en Docker-commando's bruikbaar. De eerste build downloadt de benodigde images en pakketten en kan even duren. Een koude start leest de volledige CSV in het geheugen.

## Instellingen en secrets

De standaardpoort is 8080. Voor een andere poort kopieer je `.env.example` naar `.env` en pas je `PLAYR_PORT` aan. Een `.env` is optioneel; de standaardstart hierboven werkt zonder dat bestand.

```powershell
Copy-Item .env.example .env
```

## Hoe het project in elkaar zit

| Onderdeel | Bestand(en) | Taak |
| --- | --- | --- |
| Start en HTTP-routes | `Program.cs` | Registreert de onderdelen en biedt de pagina's en `/api/recommendations` aan. |
| Data-inleeslaag | `Repositories/GameRepository.cs`, `Data/GameMap.cs`, `Models/Game.cs` | Leest `Data/games.csv` en zet CSV-kolommen om naar `Game`-objecten. |
| Aanbevelingen | `Services/BasicRecommender.cs` | Sorteert op het aantal reviews en geeft de 50 populairste spellen terug. |
| Pagina's | `Components/Pages/Home.razor`, `Home.razor.cs`, `Library.razor` | Toont de spellen en beheert zoek-, sorteer-, carousel- en detailstatus. |
| Vormgeving | `Styles/input.css`, `wwwroot/js/playr-scroll.js` | Bouwt Tailwind CSS en bedient de scrolnavigatie. |

Met .NET 8 SDK en Node.js kun je vanuit de projectmap werken:

```powershell
npm ci
npm run build:css
dotnet run
```

Ook hierbij moet `Data/games.csv` aanwezig zijn. De URL staat in de `dotnet run`-uitvoer. `npm run dev:css` houdt de CSS tijdens het aanpassen automatisch bij.
