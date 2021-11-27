module TPHA_bot.MatchCommand

open DSharpPlus.Entities
open TPHA_bot.Commands.CryptoCommand
open TPHA_bot.Commands.StockCommand
open TPHA_bot.Commands.RunListRolesCommand
open TPHA_bot.Configuration

let matchCommand (command: string) (message: DiscordMessage) (config: BotConfiguration) : Async<Result<string, string>> =
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
            | "list-roles" ->
                let! result = runListRolesCommand message config.RoleCutoff
                return result
            | _ -> return Ok "No command matched."
    }