module impl.day05

open Farkle
open Farkle.Builder
open Farkle.Builder.Regex
open Microsoft.FSharp.Core


type RangeMap = { From: Range.T; Shift: int64 }
type RangesMaps = RangeMap list

let private number = Terminals.int64 "Number"

// 79 14
let private range = "Range" ||= [
    !@ number .>>. number => Range.create
]

// 37 52 2
let private rangeMap = "Map" ||= [
    !@ number .>>. number .>>. number
        => fun dest source length -> { From = Range.create source (source + length - 1L); Shift = dest - source }
]

let private rangeMaps = "Maps" ||= [
    // soil-to-fertilizer map:\n
    !% (regexString @"\S+[ ]map:" |> terminalU "Maps header") .>> newline
        // 0 15 37
        // 37 52 2
        .>>. (sepBy1 newline rangeMap)
        |> asIs
]
let parse input =
    let blocks = input |> Pattern2.read id
    
    let seedsInput =
        blocks[0]
        |> RuntimeFarkle.parseUnsafe (
                RuntimeFarkle.build ("Seeds" ||= [
                    !& "seeds: " .>>. (many1 number) |> asIs
        ]))
        
    let mapsArrayInput = 
        blocks[1..]
        |> Seq.map (RuntimeFarkle.parseUnsafe (RuntimeFarkle.build rangeMaps))
        |> Seq.toList
    
    seedsInput, mapsArrayInput
        
module Part1 = 
  
    let solve seeds mapsList =
        let applyMap seed maps =
            match List.tryFind (fun m -> Range.contains seed m.From) maps with
            | None -> seed
            | Some map -> seed + map.Shift
        let seedToLocation seed =
            mapsList
            |> List.fold applyMap seed
        seeds |> List.map seedToLocation |> List.min
        
module Part2 =
   
    let solve seeds mapsList =
        let seedRanges =
            seeds |> Seq.chunkBySize 2
            |> Seq.map (fun x -> Range.create (x[0]) (x[1] +  x[0] - 1L))
            |> Seq.toList
        
        let applyMap maps range =
            let intersections =
                maps |> List.choose (fun map ->
                    Range.intersect map.From range
                    |> Option.map (fun intersection -> intersection, Range.shift map.Shift intersection)
                )
            if (intersections |> List.isEmpty) then [range]
            else
            let differences =
                intersections |> Seq.map fst
                |> Seq.fold (fun n m -> n |> List.collect (fun xx -> Range.subtract xx m)) [range]
              
            intersections
            |> Seq.map snd
            |> Seq.append differences 
            |> List.ofSeq
        
        let result =
            mapsList
            |> Seq.fold (fun rs map -> rs |> List.collect (applyMap map)) seedRanges
            |> Seq.map Range.start
            |> Seq.min
        
        result