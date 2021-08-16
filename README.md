# TPHA_bot

## About the Project
This is a simple discord bot written in FSharp with the help of [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus). It was written specifically for a small gaming sub I created and is mostly an exercise in building a "real-world" application in F#.

### Built With
* [F#](https://fsharp.org/)
* [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus)

## Getting Started
### Prerequisites
You'll need to have the [.NET SDK 5.0](https://dotnet.microsoft.com/download/dotnet/5.0) (not the runtime) installed.

If you want to build the docker image on your machine, you'll need to have [Docker](https://www.docker.com/products/docker-desktop) installed as well.

### Installation
1. Restore the project with:
    ```sh
    dotnet restore
    ```
2. Create a new bot account following the instructions [here](https://dsharpplus.github.io/articles/basics/bot_account.html).
3. Create a new environment variable on your system called `DISCORD_BotToken` and set it to the token you generated in the previous step.
   - Windows (CMD):
       ```shell
       SETX DISCORD_BotToken GENERATED_TOKEN
       ```
   - Mac/Linux:
       ```sh
       export DISCORD_BotToken=GENERATED_TOKEN
       ```
4. Build the application with:
    ```shell
    dotnet build
    ```
      ...or run with:
    ```shell
    dotnet run
    ```
   
## Usage and Testing
Invite the bot to a discord server with [these instructions](https://www.docker.com/products/docker-desktop) and run the bot. You can test that the bot is working by sending a message with the content `ping` to a channel in the server. You can learn about the full capabilities of the bot by using the `!help` command.

Run the unit tests with:
```shell
dotnet test --no-restore --verbosity normal
```

## Building the Docker Image
```shell
docker build .
```

## Roadmap
See the [project page](https://github.com/benwurth/TPAH_bot/projects/1).

## Contributions
Contributions are definitely appreciated!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License
See `LICENSE` for full license information.

## Contact
Ben Wurth - [ben.wurth@gmail.com](mailto:ben.wurth@gmail.com)

Project Link: https://github.com/benwurth/TPAH_bot

## Acknowledgements
* README.md heavily "inspired" by https://github.com/othneildrew/Best-README-Template