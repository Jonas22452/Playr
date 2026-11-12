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
docker compose down
```

Je `games.csv` blijft gewoon in `Data` staan.

## Andere poort

Wil je een andere poort dan 8080, kopieer `.env.example` naar `.env` en verander `PLAYR_PORT`. Het `.env`-bestand is lokaal en wordt niet gecommit.
