module TPHA_bot.Commands.StockCommand

open DSharpPlus
open TPHA_bot.Stocks.YFinanceService

let runStockCommand (commandParts: string []) (message: Entities.DiscordMessage) : Async<Result<string, string>> =
    async {
        match List.ofArray commandParts with
        | [] -> return Error "No stock passed."
        | firstPart :: _ ->
            let! stockPrice = getCurrentStockPrice firstPart
            let stockInfo = $"{firstPart}: {stockPrice}"
            let! _ = message.RespondAsync(stockInfo) |> Async.AwaitTask
            printfn $"Received request for stock command: {firstPart} | Returned: \"{stockInfo}\""
            return Ok "Message sent."
    }