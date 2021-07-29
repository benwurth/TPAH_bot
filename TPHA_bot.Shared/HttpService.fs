module TPHA_bot.Shared.HttpService

open System.Net.Http

let httpGet (url: string) (httpClient: HttpClient) =
    async {
        let! result = httpClient.GetAsync(url) |> Async.AwaitTask
        result.EnsureSuccessStatusCode |> ignore

        let! content =
            result.Content.ReadAsStringAsync()
            |> Async.AwaitTask

        return content
    }
