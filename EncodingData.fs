namespace AlphaProject
open System.Security.Cryptography
open System.Text


module EncodingData =

    let hashPassword (stringToEncode : string) =
            using (SHA256.Create()) (fun sha256 ->
                stringToEncode
                |> Encoding.UTF8.GetBytes
                |> sha256.ComputeHash
                |> Array.map (fun b -> b.ToString("x2"))
                |> String.concat ""
            )

