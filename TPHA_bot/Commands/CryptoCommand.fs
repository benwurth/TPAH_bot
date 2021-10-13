module TPHA_bot.Commands.CryptoCommand

open DSharpPlus.Entities
open TPHA_bot.Commands.StockCommand

let runCryptoCommand (args: string list) (message: DiscordMessage): Async<Result<string, string>> =
    async {
        match args with
        | [] -> return Error "No crypto ticker passed."
        | ticker :: _ ->
            do! sendStockPrice $"{ticker}-usd" message
            return Ok "Message sent."
    }