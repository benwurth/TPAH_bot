module TPHA_bot.Tests.ConfigurationTests

open System
open Microsoft.FSharp.Reflection
open NUnit.Framework
open TPHA_bot.Configuration

type TestConfiguration =
    { BotToken: string
      CommandCharacter: string }

let resultIsOk =
    function
    | Ok _ -> true
    | Error _ -> false

[<Test>]
let ``configContainsAllRequiredFields should return a Result of true when the configuration Map contains all required fields``
    ()
    =
    let configurationMap =
        [ "BotToken", "TEST"
          "CommandCharacter", "!" ]
        |> Map.ofList

    let listOfRequiredFields =
        FSharpType.GetRecordFields(typeof<TestConfiguration>)
        |> List.ofArray

    let result =
        configContainsAllRequiredFields listOfRequiredFields configurationMap

    Assert.IsTrue(resultIsOk result, "Result was not Ok")

    let getResult =
        function
        | Ok x -> x
        | Error _ -> false

    Assert.IsTrue(getResult result)

[<Test>]
let ``configContainsAllRequiredFields should return an Error when the configuration Map does not contain all required fields``
    ()
    =
    let configurationMap =
        [ "CommandCharacter", "!" ] |> Map.ofList

    let listOfRequiredFields =
        FSharpType.GetRecordFields(typeof<TestConfiguration>)
        |> List.ofArray

    let result =
        configContainsAllRequiredFields listOfRequiredFields configurationMap

    Assert.IsFalse(resultIsOk result, "Result did not Error")

    let getError =
        function
        | Ok _ -> ""
        | Error x -> x

    Assert.AreEqual(getError result, "Could not find a configuration entry for System.String BotToken")

[<Test>]
let ``tryGetConfigurationValue should properly walk the configurationType it is given and populate the configurationMap``
    ()
    =
    let mockGetValue varName =
        if varName = "BotToken" then
            Some "TEST_TOKEN"
        else
            None

    let _, result =
        tryGetConfigurationValue None (typeof<TestConfiguration>, Map.empty) mockGetValue

    let expected =
        [ "BotToken", "TEST_TOKEN" ] |> Map.ofList

    Assert.AreEqual(expected, result)

[<Test>]
let ``tryGetConfigurationValue should work properly with the optional prefix`` () =
    let mockGetValue varName =
        if varName = "PREFIX_BotToken" then
            Some "TEST_TOKEN"
        else
            None

    let _, result =
        tryGetConfigurationValue (Some "PREFIX_") (typeof<TestConfiguration>, Map.empty) mockGetValue

    let expected =
        [ "BotToken", "TEST_TOKEN" ] |> Map.ofList

    Assert.AreEqual(expected, result)

[<Test>]
let ``tryGetConfigurationValue should not overwrite existing values in the configMap`` () =
    let primaryConfigSource p (ct, cm) : Type * Map<string, string> =
        tryGetConfigurationValue
            p
            (ct, cm)
            (fun x ->
                if x = "BotToken" then
                    Some "PRIMARY_KEY"
                else
                    None)

    let secondaryConfigSource p (ct, cm) : Type * Map<string, string> =
        tryGetConfigurationValue
            p
            (ct, cm)
            (fun x ->
                if x = "BotToken" then
                    Some "SECONDARY_KEY"
                else
                    None)

    let _, result =
        primaryConfigSource None (typeof<TestConfiguration>, Map.empty)
        |> secondaryConfigSource None

    let expected =
        [ "BotToken", "PRIMARY_KEY" ] |> Map.ofList

    Assert.AreEqual(expected, result)

[<Test>]
let ``Getting an existing Environment Variable should return the string value`` () =
    let value =
        Environment.GetEnvironmentVariable("PATH")

    Assert.AreEqual(typeof<string>, value.GetType())

[<Test>]
let ``Getting an Environment Variable that does not exist should return a None`` () =
    let variableIsNone =
        match Option.ofObj (Environment.GetEnvironmentVariable("TEST")) with
        | Some _ -> false
        | None -> true

    Assert.IsTrue(variableIsNone)
