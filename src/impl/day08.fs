module impl.day08

open Farkle
open Farkle.Builder
open Farkle.Builder.Regex

type Direction = | Left | Right

let direction = "Direction" ||= [
    !& "R" =% Right
    !& "L" =% Left
]
let instructions = many1 direction

let node = regexString "[0-9A-Z]+" |> terminal "Node" (T(fun _ chars -> chars.ToString()))

let mapEntry = "MapEntry" ||= [
    !@ node .>> "=" .>> "(" .>>. node .>> "," .>>. node .>> ")"
        => fun target left right -> target, (left, right)
]

let parse input =
    let blocks = Pattern2.read id input
    let instructions =
        blocks[0]
        |> RuntimeFarkle.parseUnsafe (RuntimeFarkle.build instructions)
    let mapEntryParser = RuntimeFarkle.build mapEntry
    let parseMapEntry s =
        s
        |> RuntimeFarkle.parseUnsafe mapEntryParser
        
    let map = Pattern1.read parseMapEntry blocks[1] |> Map.ofArray
        
    instructions |> Array.ofList, map
    
    
let solve1 (instructions : Direction[]) (map : Map<string, string*string>) =
    let infiniteInstructions = Seq.initInfinite (fun i -> instructions[i % (Array.length instructions)])
    
    let infiniteNavigations = 
        infiniteInstructions
        |> Seq.scan (
            fun node instruction ->
                let entry = map[node]
                match instruction with
                | Left -> fst entry
                | Right -> snd entry
        ) "AAA"
        
    infiniteNavigations
    |> Seq.findIndex ((=)"ZZZ")
    
let solve2 (instructions : Direction[]) (map : Map<string, string*string>) =
    let infiniteInstructions = Seq.initInfinite (fun i -> instructions[i % (Array.length instructions)])
    
    let allA = map |> Map.keys |> Seq.filter (fun s -> s.EndsWith "A") |> Array.ofSeq
    let infiniteNavigations start = 
        infiniteInstructions
        |> Seq.scan (
            fun node instruction ->
                let entry = map[node]
                match instruction with
                | Left -> fst entry
                | Right -> snd entry
        ) start
            
    allA |> Array.map (fun a ->
        infiniteNavigations a
        |> Seq.findIndex (fun node -> node.EndsWith "Z") |> int64
    ) |> Array.reduce lcm