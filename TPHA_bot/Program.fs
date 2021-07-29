open System
open System.Threading.Tasks
open DSharpPlus
open DSharpPlus.EventArgs
open TPHA_bot.Configuration
open TPHA_bot.Stocks.YFinanceService

type MessageHandler = DiscordClient -> MessageCreateEventArgs -> unit

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

let checkCommandCharacter (commandCharacter: char) (message: string) : bool * string =
    if message.ToLower().StartsWith(commandCharacter) then
        true, message.Substring(1)
    else
        false, message

let runStockCommand (commandParts: string []) (message: Entities.DiscordMessage) : Async<Result<string, string>> =
    async {
        match List.ofArray commandParts with
        | [] -> return Error "No stock passed."
        | firstPart :: _ ->
            let! stockPrice = getCurrentStockPrice firstPart
            let stockInfo = $"{firstPart}: {stockPrice}"
            let! message = message.RespondAsync(stockInfo) |> Async.AwaitTask
            return Ok "Message sent."
    }

let matchCommand (command: string) (message: Entities.DiscordMessage) : Async<Result<string, string>> =
    async {
        let commandParts = command.Split(' ')

        match List.ofArray commandParts with
        | [] -> return Ok "No command passed."
        | firstPart :: remainingParts ->
            match firstPart.ToLower() with
            | "stock" ->
                let! result = runStockCommand (Array.ofList remainingParts) message
                return result
            | _ -> return Ok "No command matched."
    }

let handleCommands (c: DiscordClient) (args: MessageCreateEventArgs) : Task =
    async {
        let message = args.Message.Content

        match checkCommandCharacter '!' message with
        | true, command ->
            let! result = matchCommand command args.Message

            match result with
            | Ok "Message sent." -> return ()
            | Ok x ->
                let! _ =
                    args.Message.RespondAsync($"Error: {x}")
                    |> Async.AwaitTask

                return ()
            | Error x ->
                let! _ =
                    args.Message.RespondAsync($"Error: {x}")
                    |> Async.AwaitTask

                return ()
        | false, _ -> return ()
    }
    |> Async.StartAsTask
    :> Task

let mainAsync =
    async {
        match getConfiguration with
        | Error e -> printfn $"Could not get configuration: {e}"
        | Ok config ->
            let discordConfiguration = DiscordConfiguration()
            discordConfiguration.Token <- config.BotToken
            discordConfiguration.TokenType <- TokenType.Bot
            discordConfiguration.Intents <- DiscordIntents.AllUnprivileged
            let discord = new DiscordClient(discordConfiguration)
            discord.add_MessageCreated (fun x c -> pingMessageResponse x c)
            discord.add_MessageCreated (fun x c -> handleCommands x c)
            do! discord.ConnectAsync() |> Async.AwaitTask
            do! Task.Delay(-1) |> Async.AwaitTask
    }

[<EntryPoint>]
let main argv =
    mainAsync |> Async.RunSynchronously
    Console.Read() |> ignore
    0
