# Playr

Playr toont populaire games uit een lokale Steam-dataset. Op de startpagina kun je games zoeken, sorteren en de details bekijken. De Library-pagina is nog in ontwikkeling.

## Opstarten

Je hebt Git en een draaiende Docker Desktop nodig. De repository bevat een voorbeelddataset; `games.csv` is optioneel.

```powershell
git clone https://github.com/Jonas22452/Playr.git
cd Playr
```

Wil je de volledige dataset gebruiken, zet dan `games.csv` in de map `Data`, zodat het bestand op `Data/games.csv` staat. Playr kiest dit bestand eerst. Zonder volledige dataset gebruikt Playr automatisch `SampleData/games.csv`. De volledige dataset blijft lokaal en wordt niet in de Docker-image of repository opgenomen.

Start Playr de eerste keer met:

```powershell
docker compose up --build -d
```

Open daarna http://localhost:8080. De eerste build kan een paar minuten duren. Bij volgende keren volstaat `docker compose up -d`. Gebruik `--build` opnieuw als je de code of afhankelijkheden hebt aangepast.

## Stoppen

Voer uit:

```powershell
docker compose down
```

## Hoe het project in elkaar zit

| Onderdeel | Bestand(en) | Taak |
| --- | --- | --- |
| Start en HTTP-routes | `Program.cs` | Registreert de onderdelen en biedt de pagina's en `/api/recommendations` aan. |
| Data-inleeslaag | `Repositories/GameRepository.cs`, `Data/GameMap.cs`, `Models/Game.cs` | Leest bij voorkeur `Data/games.csv`, anders de sample, en zet CSV-kolommen om naar `Game`-objecten. |
| Aanbevelingen | `Services/BasicRecommender.cs` | Sorteert op het aantal reviews en geeft de 50 populairste spellen terug. |
| Pagina's | `Components/Pages/Home.razor`, `Home.razor.cs`, `Library.razor` | Toont de spellen en beheert zoek-, sorteer-, carousel- en detailstatus. |
| Vormgeving | `Styles/input.css`, `wwwroot/js/playr-scroll.js` | Bouwt Tailwind CSS en bedient de scrolnavigatie. |

In `Home.razor` staan korte herkenningspunten direct boven de scrolnavigatie, zijbalk, zoekbalk, uitgelichte game, spellenkaarten en het detailvenster. De bijbehorende interactie staat in `Home.razor.cs`.

## Lokaal ontwikkelen zonder Docker

Met .NET 8 SDK en Node.js kun je vanuit de projectmap werken:

```powershell
npm ci
npm run build:css
dotnet run
```

Ook hierbij heeft `Data/games.csv` voorrang op `SampleData/games.csv`. De URL staat in de `dotnet run`-uitvoer. `npm run dev:css` houdt de CSS tijdens het aanpassen automatisch bij.
