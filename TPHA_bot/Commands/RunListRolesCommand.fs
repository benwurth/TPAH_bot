module TPHA_bot.Commands.RunListRolesCommand

open DSharpPlus.Entities
open TPHA_bot.CommandUtils

let runListRolesCommand (message: DiscordMessage) (roleCutoff: Option<string>) : Async<Result<string, string>> =
    let rec skipRolesUntil cutoff (roles: DiscordRole list) =
        match roles with
        | [] -> []
        | x::y when x.Name.ToLower() = cutoff -> y
        | _::y -> skipRolesUntil cutoff y
    
    let skipUntilCutoff roles =
        match roleCutoff with
        | None -> roles
        | Some cutoff -> skipRolesUntil cutoff roles
    
    async {
        let guild = message.Channel.Guild

        let roles =
            guild.Roles
            |> Seq.map (fun p -> p.Value)
            |> Seq.filter (fun r -> r.Name.ToLower() <> "@everyone")
            |> Seq.sortByDescending (fun r -> r.Position)
            |> (fun r -> skipUntilCutoff (List.ofSeq r))
            |> Seq.map (fun r -> $"{r.Name}")
            |> String.concat "\n"

        do! sendText message roles
        printfn $"{roles}"
        return messageSentResult
    }
