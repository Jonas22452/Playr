# Playr

Playr toont populaire games uit een lokale Steam-dataset. Op de startpagina kun je games zoeken, sorteren en de details bekijken. De Library-pagina is nog in ontwikkeling.

## Opstarten

Je hebt Git, Docker Desktop en `games.csv` nodig. Zorg dat Docker Desktop draait.

```powershell
git clone https://github.com/Jonas22452/Playr.git
cd Playr
```

Zet `games.csv` in de map `Data`, zodat het bestand op `Data/games.csv` staat. De data blijft lokaal en zit niet in de repository.

Start Playr de eerste keer met:

```powershell
docker compose up --build
```

Open daarna http://localhost:8080. De eerste build kan een paar minuten duren. Bij volgende keren volstaat `docker compose up`. Gebruik `--build` opnieuw als je de code of afhankelijkheden hebt aangepast.

## Stoppen

Druk in de terminal op `Ctrl+C` en voer daarna uit:

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
