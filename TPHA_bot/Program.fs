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
        let res =
            getAppConfiguration
            |> configureDiscordBot
            |> addMessageHandler pingMessageResponse
            |> addMessageHandler handleCommands
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
