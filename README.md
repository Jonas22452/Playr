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

Zet eventuele toekomstige sleutels uitsluitend in je lokale `.env` of in een secret store. `.env` en `Data/*.csv` staan in `.gitignore` en `.dockerignore`. `.env.example` bevat alleen een voorbeeldwaarde en mag wel in Git. Voeg nooit echte sleutels toe aan `appsettings.json`, workflows of de voorbeeldfile.

## Hoe het project in elkaar zit

| Onderdeel | Bestand(en) | Taak |
| --- | --- | --- |
| Start en HTTP-routes | `Program.cs` | Registreert de onderdelen en biedt de pagina's en `/api/recommendations` aan. |
| Data-inleeslaag | `Repositories/GameRepository.cs`, `Data/GameMap.cs`, `Models/Game.cs` | Leest `Data/games.csv` en zet CSV-kolommen om naar `Game`-objecten. |
| Aanbevelingen | `Services/BasicRecommender.cs` | Sorteert op het aantal reviews en geeft de 50 populairste spellen terug. |
| Pagina's | `Components/Pages/Home.razor`, `Home.razor.cs`, `Library.razor` | Toont de spellen en beheert zoek-, sorteer-, carousel- en detailstatus. |
| Vormgeving | `Styles/input.css`, `wwwroot/js/playr-scroll.js` | Bouwt Tailwind CSS en bedient de scrolnavigatie. |

In `Home.razor` staan korte herkenningspunten direct boven de scrolnavigatie, zijbalk, zoekbalk, uitgelichte game, spellenkaarten en het detailvenster. De bijbehorende interactie staat in `Home.razor.cs`.

## Keuzes voor milestone 2

- **Eén app-container:** de webpagina en de kleine aanbevelings-API gebruiken dezelfde data en hetzelfde .NET-proces. Een extra service zou hier alleen meer opstartwerk geven.
- **Drie buildstappen in Docker:** Node maakt de Tailwind CSS, de .NET SDK bouwt de app en de kleinere ASP.NET-runtime draait hem. Zo zijn Node en de compiler niet nodig in de draaiende container.
- **Lokale CSV als alleen-lezen koppeling:** `./Data` blijft op je computer en verschijnt in de container als `/app/Data`. De app kan de brondata niet wijzigen. Daarom moet de CSV vóór het starten in `Data/games.csv` staan.
- **Vaste .NET-hoofdversie:** `Playr.csproj` gebruikt .NET 8 en de Dockerfile gebruikt dezelfde versie. Daarmee voorkom je een build die lokaal werkt maar in Docker stukloopt.
- **CSV-controle bij opstarten:** als de dataset ontbreekt, faalt de app meteen met het verwachte pad. Dat is makkelijker te begrijpen dan een lege pagina of een fout pas bij de eerste bezoeker.
- **HTTP op localhost:** Compose publiceert de container op poort 8080. Voor deze lokale milestone is geen certificaat nodig. Bij latere publieke hosting kan TLS op de reverse proxy worden ingesteld.
- **Build op elke push:** GitHub Actions bouwt de Docker-image, inclusief CSS en .NET-publicatie. De build heeft de lokale CSV niet nodig, omdat die pas bij het starten wordt gekoppeld. Pull requests krijgen dezelfde controle.

## Lokaal ontwikkelen zonder Docker

Met .NET 8 SDK en Node.js kun je vanuit de projectmap werken:

```powershell
npm ci
npm run build:css
dotnet run
```

Ook hierbij moet `Data/games.csv` aanwezig zijn. De URL staat in de `dotnet run`-uitvoer. `npm run dev:css` houdt de CSS tijdens het aanpassen automatisch bij.
