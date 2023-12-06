module impl.day06
open Farkle
open Farkle.Builder

let private number = Terminals.int64 "Number"
let private time = "Time" ||= [
    !& "Time:" .>>. (many1 number) |> asIs
]
let private distance = "Distance" ||= [
    !& "Distance:" .>>. (many1 number) |> asIs
]

let parse input =
    let lines = Pattern1.read id input
    let times = RuntimeFarkle.parseString (RuntimeFarkle.build time) lines[0] |> Result.get
    let distances = RuntimeFarkle.parseString (RuntimeFarkle.build distance) lines[1] |> Result.get
    List.zip times distances

let solve input =
    input
    |> Seq.map (fun (time, distance) ->
        seq { 1L .. time-1L }
        |> Seq.filter (fun hold -> (time - hold) * hold > distance)
        |> Seq.length
    ) |> Seq.reduce (*)
