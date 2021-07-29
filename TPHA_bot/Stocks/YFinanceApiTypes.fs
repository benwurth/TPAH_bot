module TPHA_bot.Stocks.YFinanceApiTypes

open FSharp.Json

type YFinanceTradingPeriod = {
    Timezone: string
    Start: int
    End: int
    [<JsonField("gmtoffset")>]
    GmtOffset: int
}

type YFinanceCurrentTradingPeriod = {
    Pre: YFinanceTradingPeriod
    Regular: YFinanceTradingPeriod
    Post: YFinanceTradingPeriod
}

type YFinanceChartMeta = {
    Currency: string
    Symbol: string
    ExchangeName: string
    InstrumentType: string
    FirstTradeDate: int
    RegularMarketTime: int
    [<JsonField("gmtoffset")>]
    GmtOffset: int
    Timezone: string
    ExchangeTimezoneName: string
    RegularMarketPrice: double
    ChartPreviousClose: double
    PreviousClose: double
    Scale: int
    PriceHint: int
    CurrentTradingPeriod: YFinanceCurrentTradingPeriod
    TradingPeriods: YFinanceTradingPeriod[][]
    DataGranularity: string
    Range: string
    ValidRanges: string[]
}

type YFinanceQuote = {
    High: double option []
    Low: double option []
    Open: double option []
    Close: double option []
    Volume: double option []
}

type YFinanceIndicators = {
    Quote: YFinanceQuote[]
}

type YFinanceResult = {
    Meta: YFinanceChartMeta
    Timestamp: int[]
    Indicators: YFinanceIndicators
}

type YFinanceChart = {
    Result: YFinanceResult[]
}

type YFinanceChartResult = {
    Chart: YFinanceChart
}