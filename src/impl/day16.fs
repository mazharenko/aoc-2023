module impl.day16

#nowarn "25"

open System.Collections.Generic
open Bfs.common
open Bfs.Custom

type Facing = Point

let private down : Facing = Point (1,0)
let private up : Facing = Point (-1,0)
let private right : Facing = Point (0,1)
let private left : Facing = Point (0,-1)

let parse input =
    input
    |> Pattern1.read Seq.toArray
    |> array2D
    
let private beamChange (beam, c) =
    match c, beam with
    | '.', _
    | '|', Eq up    | '|',  Eq down | '-', Eq right | '-', Eq left -> [beam]
    | '/', Eq down  | '\\', Eq up -> [left]
    | '/', Eq up    | '\\', Eq down -> [right]
    | '/', Eq left  | '\\', Eq right -> [down]
    | '/', Eq right | '\\', Eq left -> [up]
    | '|', Eq right | '|',  Eq left -> [up; down]
    | '-', Eq up    | '-',  Eq down -> [right; left]
    
// the BFS algorithm this part relies on implies that the target might not be found.
// in this case it returns all the paths it has traversed until it decided that the
// process could no longer continue.

type private Beam = { Position: Point; Direction: Facing }
// State type is the graph node.
type private State = { Beam: Beam; Contraption: char[,]; }
// Contraption never changes, we can consider only Beam when checking if a node
// has been already visited.
let private settings = { VisitedKey = _.Beam }
// with the target function like the following, the BFS will never "find" anything,
// but traverse the whole graph instead 
let private target _ = false
// adjacent nodes are determined dynamically. it does not need to check if a beam
// reaches the same way faced beam at the same position. this is going to be handled
// by the BFS algorithm itself as already visited nodes.
let private adjacency state =
    beamChange (state.Beam.Direction, (state.Contraption |> Array2D.atPoint state.Beam.Position))
    |> List.map (fun facing -> { Position = facing + state.Beam.Position; Direction = facing })
    |> List.where (fun beam -> Array2D.tryAtPoint beam.Position state.Contraption |> Option.isSome)
    |> List.map (fun beam -> { state with Beam = beam })

// this is a wierd part. BFS works relatively fast. it returns a list of lists of
// traversed nodes. then we want to count unique coordinates in all the paths, and
// counting elements in lists is way too slow.
// here, we rely on how the BFS builds the path lists. when two adjacent nodes
// found, two corresponding lists are created with the same tail, representing
// the path to the current node.
//
//     new1   <─────┐
//                current  <───  previous
//     new2   <─────┘
//
// countUniqueElements collects all coordinates but makes sure that each
// tail is processed only once
let private countUniqueElements lists =
    let used = HashSet<_>(ReferenceEqualityComparer.Instance)
    let set = HashSet<Point>()
    let rec addToSet list =
        match list with
        | [] -> ()
        | x::xs ->
            if used.Add list then
                set.Add x.Beam.Position |> ignore
                addToSet xs
    List.iter addToSet lists
    set.Count

let solve1 input =
    let initialState = { Beam = {Position = Point(0,0); Direction = right }; Contraption = input; }
    let (NotFound(paths)) = findPath settings { Adjacency = adjacency } initialState target
    countUniqueElements paths
    
let solve2 input =
    let startBeams = seq {
       yield! input |> Array2D.cols |> Seq.collect(fun j -> [ {Position = Point(0, j); Direction =  down}; {Position = Point((Array2D.length1 input) - 1, j); Direction =  up}])
       yield! input |> Array2D.rows |> Seq.collect(fun i -> [ {Position = Point(i, 0); Direction =  right};{Position = Point(i, (Array2D.length2 input) - 1); Direction =  left}])
    }
    startBeams
    |> Seq.map (fun startBeam ->
        let initialState = { Beam = startBeam; Contraption = input; }
        let (NotFound(paths)) = findPath settings { Adjacency = adjacency } initialState target
        countUniqueElements paths
    ) |> Seq.max
     