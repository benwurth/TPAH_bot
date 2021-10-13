module TPHA_bot.Commands.StockCommand

open DSharpPlus.Entities
open TPHA_bot.Stocks.YFinanceService
open TPHA_bot.CommandUtils

let sendStockPrice ticker message =
    async {
        let! stockPrice = getCurrentStockPrice ticker
        let stockInfo = $"{ticker}: ${stockPrice}"
        do! sendText message stockInfo
        printfn $"Received request for stock command: {ticker} | Returned: \"{stockInfo}\""
    }

let runStockCommand (commandParts: string []) (message: DiscordMessage) : Async<Result<string, string>> =
    let sendRopeGif =
        async {
            do! sendText message "https://tenor.com/view/hang-suizide-desgosto-suicide-rope-gif-15405316"
            printfn "Received request for stock command: $ROPE | Returned an edgy gif."
        }

    async {
        match List.ofArray commandParts with
        | [] -> return Error "No stock ticker passed."
        | firstPart :: _ ->
            match firstPart.ToLower() with
            | "rope" -> do! sendRopeGif
            | _ -> do! sendStockPrice firstPart message

            return Ok "Message sent."
    }
