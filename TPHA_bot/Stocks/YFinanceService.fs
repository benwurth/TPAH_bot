module TPHA_bot.Stocks.YFinanceService

open FSharp.Json
open TPHA_bot.Stocks.YFinanceApiTypes
open TPHA_bot.Shared.HttpService

let getStockChart (stockTicker: string) : Async<YFinanceChartResult> =
    async {
        let url =
            $"https://query2.finance.yahoo.com/v8/finance/chart/{stockTicker.ToLower()}"

        let! content = httpGet url (new System.Net.Http.HttpClient())
        let config = JsonConfig.create(jsonFieldNaming = Json.lowerCamelCase)
        let financeResult =
            Json.deserializeEx<YFinanceChartResult> config content

        return financeResult
    }
    

let getCurrentStockPrice stockTicker =
    async {
        let! chartResult = getStockChart stockTicker
        return chartResult.Chart.Result.[0].Meta.RegularMarketPrice
    }