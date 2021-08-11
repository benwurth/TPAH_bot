module TPHA_bot.Configuration

open System
open System.Reflection
open Microsoft.FSharp.Reflection

type BotConfiguration = { BotToken: string }

let rec configContainsAllRequiredFields (recordFields: PropertyInfo list) config : Result<bool, string> =
    match recordFields with
    | [] -> Ok true
    | head :: tail ->
        if not (Map.exists (fun k _ -> head.Name = k) config) then
            Error $"Could not find a configuration entry for {head}"
        else
            configContainsAllRequiredFields tail config

let buildBotConfiguration (configMap: Map<string, string>) =
    { BotToken = configMap.Item("BotToken") }

let validateConfiguration (configType, config: Map<string, string>) =
    let recordFields = FSharpType.GetRecordFields(configType)

    match (configContainsAllRequiredFields (List.ofArray recordFields) config) with
    | Ok _ -> Ok(buildBotConfiguration config)
    | Error x -> Error x

let tryGetConfigurationValue
    optionalPrefix
    (configurationType: Type, configurationMap)
    (getVarResult: string -> string option)
    : Type * Map<string, string> =
    let addPrefix (p: Option<string>) (s: string) : string =
        match p with
        | Some x -> x + s
        | None _ -> s

    let configurationFieldNames =
        FSharpType.GetRecordFields(configurationType)
        |> List.ofArray
        |> List.map (fun p -> p.Name)

    let rec walkNames names configMap =
        let addEnvVar (fieldName: string) cMap : Map<string, string> =
            let varName = addPrefix optionalPrefix fieldName

            match getVarResult varName with
            | Some value ->
                match Map.tryFindKey (fun k _ -> k = varName) cMap with
                | Some _ -> cMap
                | None -> Map.add fieldName value cMap
            | None -> cMap

        match names with
        | [] -> configMap
        | head :: tail -> walkNames tail (addEnvVar head configMap)

    (configurationType, walkNames configurationFieldNames configurationMap)

let tryGetEnvironmentVariableConfiguration
    optionalPrefix
    (configurationType: Type, configurationMap: Map<string, string>)
    =
    tryGetConfigurationValue
        optionalPrefix
        (configurationType, configurationMap)
        (fun s -> Option.ofObj (Environment.GetEnvironmentVariable(s)))
        
let wrapConfigurationErrorMessage result =
    match result with
    | Error e -> Error $"Could not get configuration: {e}"
    | Ok x -> Ok x

let getAppConfiguration =
    tryGetEnvironmentVariableConfiguration (Some "DISCORD_") (typeof<BotConfiguration>, Map.empty)
    //    |> tryGetJsonConfiguration "appsettings.json"
    |> validateConfiguration
    |> wrapConfigurationErrorMessage
