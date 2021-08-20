# EpicBundle-FreeGames-dotnet

Fetch free games info from EpicBundle, send notification through Telegram, Bark, Email, QQ and PushPlus.

Demo Telegram Channel [@azhuge233_FreeGames](https://t.me/azhuge233_FreeGames)

## Build

Install dotnet 5.0 SDK first, you can find installation packages/guides [here](https://dotnet.microsoft.com/download).

Follow commands will publish project as a trimmed executable file.

```shell
git clone https://github.com/azhuge233/EpicBundle-FreeGames-dotnet.git
cd EpicBundle-FreeGames-dotnet
# windows, macOS
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/...]
# set PublishTrimmed property to false in Linux
dotnet publish -c Release -o /your/path/here -r linux-x64 -p:PublishTrimmed=false

```

## Usage

Fill your telegram bot token and chat ID in Config/config.json.

Check [wiki](https://github.com/azhuge233/SteamDB-FreeGames-dotnet/wiki/Config-Description) for more explanations, only Telegram, Bark and Email varaibles are available for this project.

## My Free Games Collection

- SteamDB
    - [https://github.com/azhuge233/SteamDB-FreeGames](https://github.com/azhuge233/SteamDB-FreeGames)(Archived)
    - [https://github.com/azhuge233/SteamDB-FreeGames-dotnet](https://github.com/azhuge233/SteamDB-FreeGames-dotnet)
- EpicBundle
    - [https://github.com/azhuge233/EpicBundle-FreeGames](https://github.com/azhuge233/EpicBundle-FreeGames)(Archived)
    - [https://github.com/azhuge233/EpicBundle-FreeGames-dotnet](https://github.com/azhuge233/EpicBundle-FreeGames-dotnet)
- Indiegala
    - [https://github.com/azhuge233/IndiegalaFreebieNotifier](https://github.com/azhuge233/IndiegalaFreebieNotifier)
- GOG
    - [https://github.com/azhuge233/GOGGiveawayNotifier](https://github.com/azhuge233/GOGGiveawayNotifier)
