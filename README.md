# Playr

Playr toont populaire games uit een lokale Steam-dataset. Op de startpagina kun je games zoeken, sorteren en de details bekijken. De Library-pagina is nog in ontwikkeling.

## Opstarten

Je hebt Git, Docker Desktop en `games.csv` nodig. Zorg dat Docker Desktop draait.

```powershell
git clone https://github.com/Jonas22452/Playr.git
cd Playr
```

Zet `games.csv` in de map `Data`, zodat het bestand op `Data/games.csv` staat. De data blijft lokaal en zit niet in de repository.

Maak daarna zelf een `.env`-bestand aan op basis van het voorbeeld en vul je eigen gewenste poort in:

```powershell
Copy-Item .env.example .env
```

Open `.env` en pas `PLAYR_PORT` aan als je een andere vrije poort wilt. `.env.example` bevat alleen een voorbeeldwaarde; `.env` blijft lokaal en wordt niet gecommit.

Start Playr de eerste keer met:

```powershell
docker compose up --build
```

Open daarna `http://localhost:<PLAYR_PORT>` (met de voorbeeldwaarde: http://localhost:8080). De eerste build kan een paar minuten duren. Bij volgende keren volstaat `docker compose up`. Gebruik `--build` opnieuw als je de code of afhankelijkheden hebt aangepast.

Docker start twee containers: `frontend` levert de statische bestanden en stuurt Blazor-verzoeken door; `backend` draait de C#-app en leest `Data/games.csv`.

## Stoppen

Druk in de terminal op `Ctrl+C` en voer daarna uit:

```powershell
docker compose down
```

Je `games.csv` blijft gewoon in `Data` staan.
