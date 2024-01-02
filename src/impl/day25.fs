module impl.day25

#nowarn "25"

open System
open Farkle
open Farkle.Builder
open Farkle.Builder.Regex

let private node =
    chars PredefinedSets.AllLetters |> repeat 3
    |> terminal "Component" (T(fun _ c -> new string(c)))
let private edges = "Connection" ||= [
    !@ node .>> ":" .>>. many1 node
        => fun from to' -> List.map (fun x -> from, x) to'
]

type G = { Vertexes: Map<string, int>; Edges: (string * string)[]}

let parse input =
    let parser = RuntimeFarkle.build edges
    let edges = 
        input
        |> Pattern1.read (RuntimeFarkle.parseUnsafe parser)
        |> Array.collect List.toArray
    let vertexes =
        edges
        |> Seq.collect (fun (from, to') -> seq {from;to'})
        |> Seq.distinct
    { Vertexes = vertexes |> Seq.map (fun x -> x, 1) |> Map.ofSeq; Edges = edges }

let private random = Random(2024) //random, but deterministic

// Karger's algorithm
let rec private cut (g : G) : G =
    if Map.count g.Vertexes <= 2 then g
    else
        let [|randomEdge|] = random.GetItems(g.Edges,1)
        let v1,v2 = randomEdge
        let newEdges =
            g.Edges
            |> Array.choose(fun (x, y as p) ->
                if x = v1 && y = v2 || x = v2 && y = v1 then None 
                elif x = v2 || x = v1 then Some (v1, y)
                elif y = v2 || y = v1 then Some (x, v1)
                else Some p
            )
        cut { Edges = Array.ofSeq newEdges
              Vertexes = g.Vertexes |> Map.add v1 (g.Vertexes[v1] + g.Vertexes[v2]) |> Map.remove v2 }

let solve1 input =
    let g =
        Seq.initInfinite(fun _ -> cut input)
        |> Seq.where (fun g -> g.Edges.Length = 3)
        |> Seq.head
    g.Vertexes |> Map.values |> Seq.reduce (*)
    