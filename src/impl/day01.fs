module impl.day01

open System

let parse input =
    Pattern1.read id input
    
let solve1 : string[] -> int =
    fun input ->
        input
        |> Array.map (fun line -> line |> Seq.toArray |> Array.filter Char.IsDigit)
        |> Array.map (fun c -> string c[0] + string c[^0])
        |> Array.map int
        |> Array.sum
        
let solve2 : string[] -> int =
    fun input ->
        let stringToDigit =
            [
                ("one", 1)
                ("two", 2)
                ("three", 3)
                ("four", 4)
                ("five", 5)
                ("six", 6)
                ("seven", 7)
                ("eight", 8)
                ("nine", 9)
            ] @ [
                 for i in 1..9 do
                    yield (string i, i)
            ]
        
        let calibration (s : string) =
            let firstDigit =
                stringToDigit
                // protect from deciding not found index (-1) the least
                |> Seq.minBy (fun (pattern, _) -> pattern |> String.indexOf s |> Option.defaultValue Int32.MaxValue)
                |> snd
            let lastDigit =
               stringToDigit
                // not found index (-1) is already less than any other found index
                |> Seq.maxBy (fun (pattern, _) -> s.LastIndexOf(pattern))
                |> snd
            firstDigit * 10 + lastDigit
        input
        |> Array.map calibration
        |> Array.sum