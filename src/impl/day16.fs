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
    
type private Beam = { Position: Point; Direction: Facing }
// State type is the graph node.
type private State = { Beam: Beam; Contraption: char[,]; }
// Contraption never changes, we can consider only Beam when checking if a node
// has been already visited.
let private settings = { VisitedKey = _.Beam }
// adjacent nodes are determined dynamically. it does not need to check if a beam
// reaches the same way faced beam at the same position. this is going to be handled
// by the BFS algorithm itself as already visited nodes.
let private adjacency state =
    beamChange (state.Beam.Direction, (state.Contraption |> Array2D.atPoint state.Beam.Position))
    |> List.map (fun facing -> { Position = facing + state.Beam.Position; Direction = facing })
    |> List.where (fun beam -> Array2D.tryAtPoint state.Contraption beam.Position |> Option.isSome)
    |> List.map (fun beam -> { state with Beam = beam })
// as we traverse the graph, fold its nodes to a set
let private graphFolder reachedSet (reached: Path<State>) =
    Set.add reached.Head.Item.Beam.Position reachedSet, Continue

let solve1 input =
    let initialState = { Beam = {Position = Point(0,0); Direction = right }; Contraption = input; }
    fold settings { Adjacency = adjacency } initialState graphFolder Set.empty
    |> Set.count
    
let solve2 input =
    let startBeams = seq {
       yield! input |> Array2D.cols |> Seq.collect(fun j -> [ {Position = Point(0, j); Direction =  down}; {Position = Point((Array2D.length1 input) - 1, j); Direction =  up}])
       yield! input |> Array2D.rows |> Seq.collect(fun i -> [ {Position = Point(i, 0); Direction =  right};{Position = Point(i, (Array2D.length2 input) - 1); Direction =  left}])
    }
    startBeams
    |> Seq.map (fun startBeam ->
        let initialState = { Beam = startBeam; Contraption = input; }
        fold settings { Adjacency = adjacency } initialState graphFolder Set.empty
        |> Set.count
    ) |> Seq.max
     