module impl.day12

#nowarn "25"

open System
open Microsoft.FSharp.Core

let parse input =
    input
    |> Pattern1.read (splitToTuple2 [|" "|])
    |> Array.map (fun (x, y) -> x, y.Split "," |> Array.map int)

type private State = { Springs: char list; Sizes: int list; InGroup: bool }

module private State =
    let useSizeOnGroup state =
        let { Sizes = size::sizes; Springs = _::springs } = state
        { Sizes = size - 1 :: sizes; Springs = springs; InGroup = true }
    let skipSpring state =
        { state with Springs = List.tail state.Springs }
    
let private solve =
    fun f state -> 
        match state.Springs, state.Sizes, state.InGroup with
        // reached end
        | [], [], _ -> 1L
        // out of sizes, but met #
        | '#'::_, [], _ -> 0L
        // out of sizes, just skip spring
        // if there are #, they will be validated later
        | '?'::_, [], _
        | '.'::_, [], _ ->
            state |> State.skipSpring
            |> f
        // current group ended, but met new #
        | '#'::_, 0::_, true ->
            0L
        // current group ended, not #, skip to make a gap
        | [] as springs, 0::restSizes, true
        | _::springs, 0::restSizes, true ->
            { Springs = springs; Sizes = restSizes; InGroup = false }
            |> f
        // still unspent size, but met .
        | '.'::_, _, true ->
            0L
        // still unspent size, so need to keep building group
        | '?'::_, _, true
        // still unspent size, met #, so keep building / begin group
        | '#'::_, _, _ ->
            state |> State.useSizeOnGroup
            |> f
        // not building group, so skip .
        | '.'::_, _, false ->
            state |> State.skipSpring
            |> f
        // not building group, so for ? consider two alternatives: begin a group or skip
        | '?'::_, _, false ->
            (
                state |> State.useSizeOnGroup
                |> f
            ) + (
                state |> State.skipSpring
                |> f
            )
        // reached end, but still unspent size
        | [], _, _ -> 0L

let private solveF() =
    memorec solve id
    
let solve1 (input: (string * int array)[]) =
    input
    |> Seq.sumBy (fun x -> solveF() { Sizes = snd x |> Array.toList; Springs = fst x |> Seq.toList; InGroup = false })
    
let solve2 (input: (string*int[])[]) =
    let replicatedInput = input |> Array.map (fun (a,b) ->
        let springs = String.Join('?', Array.replicate 5 a)
        let sizes = (Array.replicate 5 b) |> Array.collect id
        springs, sizes
    )
    replicatedInput
    |> Seq.map (fun x ->
        let solveMemoized = solveF()
        solveMemoized { Sizes = snd x |> Array.toList; Springs = fst x |> Seq.toList ; InGroup = false })
    |> Seq.sum