namespace TPHA_bot.Shared

module Utils =
    let classToOption = function
        | null -> None
        | x -> Some x

    let nullableToOption (n: System.Nullable<'a>) : Option<'a> =
        if n.HasValue then
            Some n.Value
        else
            None
