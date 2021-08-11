module TPHA_bot.DiscordGlue

open System.Threading.Tasks
open DSharpPlus
open DSharpPlus.EventArgs
open TPHA_bot.Configuration
open TPHA_bot.CommandUtils

type MessageHandler = DiscordClient -> MessageCreateEventArgs -> unit

let configureDiscordBot (configurationResult: Result<BotConfiguration, string>) =
    match configurationResult with
    | Error x -> Error x
    | Ok config ->
        let discordConfiguration = DiscordConfiguration()
        discordConfiguration.Token <- config.BotToken
        discordConfiguration.TokenType <- TokenType.Bot
        discordConfiguration.Intents <- DiscordIntents.AllUnprivileged
        Ok (new DiscordClient(discordConfiguration))

let pingMessageResponse (_: DiscordClient) (a: MessageCreateEventArgs) : Task =
    async {
        if a.Message.Content.ToLower().StartsWith("ping") then
            do!
                (a.Message.RespondAsync("pong!")
                 |> Async.AwaitTask
                 |> Async.Ignore)
    }
    |> Async.StartAsTask
    :> Task

let addMessageHandler handler (dcRes: Result<DiscordClient, string>) =
    match dcRes with
    | Error x -> Error x
    | Ok discordClient ->
        discordClient.add_MessageCreated (fun x c -> handler x c)
        Ok discordClient

let handleCommands _ (args: MessageCreateEventArgs) : Task =
    async {
        let message = args.Message.Content

        match checkCommandCharacter '!' message with
        | true, command ->
            let! result = matchCommand command args.Message

            match result with
            | Ok "Message sent." -> return ()
            | Ok x ->
                printfn $"Command character was detected, but unknown result was returned: {x}"
                return ()
            | Error x ->
                let errorMessage = $"ERROR: {x}"
                printfn $"{errorMessage}"
                let! _ =
                    args.Message.RespondAsync(errorMessage)
                    |> Async.AwaitTask
                return ()
        | false, _ -> return ()
    }
    |> Async.StartAsTask
    :> Task