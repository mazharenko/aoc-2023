module impl.day23

open System.Linq
open System.Collections.Generic
open Bfs.Custom

let parse input =
    Pattern1.read Seq.toArray input
    |> array2D
    
type private State = { Position: Point; Previous: Point; Map: char[,]; }

// as there is a lot of "corridors" in the initial maps, it can be reasonable to identify them and consider
// a whole "corridor" as a single edge
let private buildCompressedGraph start target (adjacency : Adjacency<State>) =
    let visitedEdges = HashSet<Point*Point>()
    let optimizedEdges = HashSet<Point*Point*int>()
    // stack to ensure tail recursion
    // in the stack we collect tuples: previous valuable node, current node to consider, and distance between them
    let rec build statesStack =
        match statesStack with
        | [] -> []
        | (prev, current, weight)::stack  ->
            let buildFrom adj =
                adj |> List.choose (fun a ->
                    if visitedEdges.Add(current.Position, a.Position) then Some (current,a,1)
                    else None
                )
            let newAdj = adjacency current
            let backAdj, forwardAdj = List.partition (fun a -> a.Position = current.Previous) newAdj
            let addToStack = 
                if target current then
                    optimizedEdges.Add (prev.Position, current.Position, weight) |> ignore
                    backAdj |> buildFrom
                else 
                    match forwardAdj with
                    | [singleAdj] ->
                        if (visitedEdges.Add (current.Position, singleAdj.Position)) then
                            [prev,singleAdj,weight+1]
                        else []
                    | [] ->
                        // it's not really correct to ignore dead-ends when building a graph
                        // but we won't need them later on anyway
                        []
                    | manyAdj ->
                        optimizedEdges.Add (prev.Position, current.Position, weight) |> ignore
                        (buildFrom manyAdj, buildFrom backAdj)
                        ||> List.append
            build (addToStack @ stack)
                
    build [start,start,0] |> ignore
    optimizedEdges
    |> Seq.group (fun (from,_,_) -> from)
           (Seq.map (fun (_, to', weight) -> to', weight)
                >> Seq.sortByDescending snd
                >> Seq.distinct
                >> List.ofSeq)
    |> Map.ofSeq
    
let private findLongestPath (adjacencyMap : Map<Point, (Point*int) list>) targetX =
    // assign an index to every point
    // will use these indexes to fill a visited nodes bit mask
    let nodeIndexes =
        adjacencyMap |> Map.values |> Seq.collect id |> Seq.map fst
        |> Seq.append (adjacencyMap |> Map.keys)
        |> Seq.distinct
        |> Seq.mapi (fun i x -> x,i) |> Map.ofSeq
    let adjIndexDictionary =
        adjacencyMap |> Map.toSeq
        |> Seq.map (fun (p, x) -> struct (nodeIndexes[p], x |> List.map(fun (a, weight) -> nodeIndexes[a], weight)))
        |> Enumerable.ToDictionary
    let startIndex = nodeIndexes[Point(0,1)]
    let targetIndex = nodeIndexes |> Map.pick (fun p i -> if Point.x p = targetX then Some i else None)
    let rec find current visited =
        if current = targetIndex then 0
        else
            let neighbors = adjIndexDictionary[current]
            neighbors
            |> Seq.map(fun (to', weight) ->
                if (visited &&& (1L <<< to') <> 0L) then 0
                else weight + find to' (visited ||| (1L <<< current))
            ) |> Seq.fold max 0
    find startIndex (1L <<< startIndex)

let solve1 input =
     let adjacency ({ Position = Point(i,j) as p; Map = map; } as state) =
         let neighbors = 
             Array2D.Adj.indexesValues4 map (i,j)
             |> List.where(fun ((adji, adjj), adj) ->
                 if adj = '#' then false
                 else match map[i,j] with
                      | '<' -> adjj < j
                      | '>' -> adjj > j
                      | '^' -> adji < i
                      | 'v' -> adji > i
                      | _ -> true
             )
         match neighbors with
         | [] -> []
         | [(ij, _)] ->
            [ { state with Position = Point(ij); Previous = p } ]
         | manyNeighbors ->
            manyNeighbors
            |> List.map (fun (ij, _) -> { Position = Point(ij); Previous = p; Map = map; })
     let start = { Position = Point(0,1); Previous = Point(0,1); Map = input;}
     let graph = buildCompressedGraph start (fun s -> s.Position |> Point.x = Array2D.length1 input - 1) adjacency 
     findLongestPath graph (Array2D.length1 input - 1)

let solve2 input =
    let adjacency ({ Position = Point(i,j) as p; Map = map; } as state) =
        let neighbors =
            Array2D.Adj.indexesValues4 map (i,j)
            |> List.filter (snd >> (<>)'#')
        match neighbors with
        | [] -> []
        | [(ij, _)] ->
            [ { state with Position = Point(ij); Previous = p } ]
        | manyNeighbors ->
            manyNeighbors
            |> List.map (fun (ij, _) -> { Position = Point(ij); Previous = p; Map = map; })
    let start = { Position = Point(0,1); Previous = Point(0,1); Map = input;}
    let graph = buildCompressedGraph start (fun s -> s.Position |> Point.x = Array2D.length1 input - 1) adjacency 
    findLongestPath graph (Array2D.length1 input - 1)