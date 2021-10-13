module TPHA_bot.MatchCommand

open DSharpPlus.Entities
open TPHA_bot.Commands.CryptoCommand
open TPHA_bot.Commands.StockCommand

let matchCommand (command: string) (message: DiscordMessage) : Async<Result<string, string>> =
    async {
        let commandParts = command.Split(' ')

        match List.ofArray commandParts with
        | [] -> return Ok "No command passed."
        | firstPart :: remainingParts ->
            match firstPart.ToLower() with
            | "stock" ->
                let! result = runStockCommand (Array.ofList remainingParts) message
                return result
            | "crypto" ->
                let! result = runCryptoCommand remainingParts message
                return result
            | _ -> return Ok "No command matched."
    }