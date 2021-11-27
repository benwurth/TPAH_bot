open System
open System.Threading.Tasks
open DSharpPlus
open TPHA_bot.Configuration
open TPHA_bot.DiscordGlue

let logErrors (res: Result<DiscordClient, string>) : Result<DiscordClient, string> =
    match res with
    | Ok x -> Ok x
    | Error x ->
        printfn $"{x}"
        Error x

let mainAsync =
    async {
        let botConfigurationResult = getAppConfiguration
        match botConfigurationResult with
        | Error e ->
            printfn $"There was an error: {e}"
            ()
        | Ok config ->
//            let handleCommandsGood client args: Task =
//                handleCommands client args config
            
            let res =
                botConfigurationResult
                |> Result.bind configureDiscordBot
                |> Result.bind (addMessageHandler pingMessageResponse)
                |> Result.bind (addMessageHandler (fun c -> handleCommands c config))
                |> logErrors

            match res with
            | Error _ -> ()
            | Ok discord ->
                do! discord.ConnectAsync() |> Async.AwaitTask
                do! Task.Delay(-1) |> Async.AwaitTask
    }

[<EntryPoint>]
let main argv =
    mainAsync |> Async.RunSynchronously
    Console.Read() |> ignore
    0
