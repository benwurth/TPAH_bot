module TPHA_bot.DiscordGlue

open System.Threading.Tasks
open DSharpPlus
open DSharpPlus.EventArgs
open TPHA_bot.Configuration
open TPHA_bot.CommandUtils
open TPHA_bot.MatchCommand

type MessageHandler = DiscordClient -> MessageCreateEventArgs -> unit

let configureDiscordBot (config: BotConfiguration) =
    let discordConfiguration = DiscordConfiguration()
    discordConfiguration.Token <- config.BotToken
    discordConfiguration.TokenType <- TokenType.Bot
    discordConfiguration.Intents <- DiscordIntents.AllUnprivileged
    Ok(new DiscordClient(discordConfiguration)  )

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

let addMessageHandler handler (discordClient: DiscordClient) =
    discordClient.add_MessageCreated (fun x c -> handler x c)
    Ok discordClient

let handleCommands _ (config: BotConfiguration) (args: MessageCreateEventArgs) : Task =
    async {
        let message = args.Message.Content

        match checkCommandCharacter '!' message with
        | true, command ->
            let! result = matchCommand command args.Message config

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
