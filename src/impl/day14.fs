module impl.day14

open System
open Microsoft.FSharp.Core

let parse input =
    input
    |> Pattern1.read Seq.toArray
    |> array2D
    
let rollLeft (field: char[,]) =
    field |> Array2D.toJagged
    |> Array.map (fun row ->
        String.Join('#',
                    (new string(row)).Split('#')
                    // 'O' > '.'
                    |> Seq.map (Seq.sortDescending >> Seq.toArray >> String.fromChars)
                )
    ) |> array2D
    
let roll4 field =
    field
    |> Array2D.rotateCCW
    |> rollLeft
    |> Array2D.rotateCW
    |> rollLeft
    |> Array2D.rotateCW
    |> rollLeft
    |> Array2D.rotateCW
    |> rollLeft
    |> Array2D.rotateCW
    |> Array2D.rotateCW

let load m =
    m |> Array2D.transpose
    |> Array2D.toJagged
    |> Array.map (fun col ->
        col |> Seq.indexed |> Seq.where (snd >> ((=)'O'))
        |> Seq.map fst
        |> Seq.map (fun i -> col.Length - i)
        |> Seq.sum
    )
    |> Array.sum
    

let solve1 input =
    input
        |> Array2D.rotateCCW
        |> rollLeft
        |> Array2D.rotateCW
        |> load

type private State = { Map: Map<char[,], int>; Matrix: char[,]; Index: int }
let rec private findCycle state =
    match Map.tryFind state.Matrix state.Map with
        | None -> findCycle { Matrix = (roll4 state.Matrix); Index = state.Index + 1; Map = Map.add state.Matrix state.Index state.Map }
        | Some found -> found, (state.Index - found), state.Map
    
let solve2 input =
    let cycleStart, cycleSize, map =
        findCycle { Map = Map.empty; Matrix = input; Index = 0 }
    let index = ((1000000000 - cycleStart) % cycleSize) + cycleStart
    map |> Map.findKey (fun _ i -> i = index)
    |> load 