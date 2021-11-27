module TPHA_bot.CommandUtils

open DSharpPlus.Entities

let checkCommandCharacter (commandCharacter: char) (message: string) : bool * string =
    if message.ToLower().StartsWith(commandCharacter) then
        true, message.Substring(1)
    else
        false, message

let sendText (message: DiscordMessage) (text: string) =
    async {
        let! _ = message.RespondAsync(text) |> Async.AwaitTask
        return ()
    }

let messageSentResult =
    Ok "Message sent."
