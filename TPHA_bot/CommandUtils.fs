module TPHA_bot.CommandUtils

open DSharpPlus
open TPHA_bot.Commands.StockCommand

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
    
let checkCommandCharacter (commandCharacter: char) (message: string) : bool * string =
    if message.ToLower().StartsWith(commandCharacter) then
        true, message.Substring(1)
    else
        false, message