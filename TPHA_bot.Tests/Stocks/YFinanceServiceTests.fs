module TPHA_bot.Tests.Stocks.YFinanceServiceTests

open NUnit.Framework
open TPHA_bot.Stocks.YFinanceService

[<Test>]
let ``Calling the YFinance api should return a correctly deserialized response``() =
    let ticker = "goog"
    let result = getStockChart ticker |> Async.RunSynchronously
    Assert.AreEqual("goog", result.Chart.Result.[0].Meta.Symbol.ToLower())