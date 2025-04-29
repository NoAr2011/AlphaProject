namespace AlphaProject

open WebSharper

[<JavaScript>]
module StringValidation =   

    let forbiddenChars = [ '*'; '='; '"'; '\''; ';' ]

    let removeForbiddenCharacters (input: string) : string =
        input
        |> Seq.filter (fun c -> not (List.contains c forbiddenChars))
        |> Seq.toArray
        |> System.String
            
        

