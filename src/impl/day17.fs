module impl.day17

#nowarn "25"
open Bfs.Custom
open Farkle

let parse input =
    input
    |> Pattern1.read Seq.toArray
    |> array2D
    |> Array2D.map (string >> int)

type State = { Map: int[,]; Position: Point; Direction: Point; StraitTiles: int }
module State =
    let goRight state =
        if state.Direction = Point.Zero then invalidOp "Cannot go right with Zero direction"
        let dir = Point.Rotation.rotateCW state.Direction
        { state with Position = dir + state.Position; StraitTiles = 1; Direction = dir }
    let goLeft state =
        if state.Direction = Point.Zero then invalidOp "Cannot go left with Zero direction"
        let dir = Point.Rotation.rotateCCW state.Direction
        { state with Position = dir + state.Position; StraitTiles = 1; Direction = dir }
    let goStraight state =
        if state.Direction = Point.Zero then invalidOp "Cannot go straight with Zero direction"
        { state with Position = state.Position + state.Direction; StraitTiles = state.StraitTiles + 1 }

let solve1 input =
    let adj state =
        let nextStates =
            [ State.goStraight; State.goLeft; State.goRight ]
            |> List.map (fun f -> f state)
        nextStates
        |> List.filter (fun s -> s.StraitTiles <= 3)
        |> List.choose (fun s -> s.Map |> Array2D.tryAtPoint s.Position |> Option.map(fun weight -> s, weight))
    
    let initialState = { Position = Point(0,0); Map = input; Direction = Point.right(); StraitTiles = 0 }
    let settings = { VisitedKey = fun s -> s.Position,s.Direction,s.StraitTiles }
    let target = fun s -> s.Position = Point(Array2D.length1 s.Map - 1, Array2D.length2 s.Map - 1)
    let (Found(path)) = findPath settings { Adjacency = adj } initialState target
    path.Head.Len
    
let solve2 input =
    
    let target = fun s -> s.Position = Point(Array2D.length1 s.Map - 1, Array2D.length2 s.Map - 1)
    let adj state =
        let nextStates =
            [
                if state.Direction = Point.Zero then
                    yield! [Point.down(); Point.left(); Point.right(); Point.up()]
                    |> List.map (fun dir -> {state with Direction = dir })
                    |> List.map State.goStraight
                else 
                    if state.StraitTiles >= 4 then
                        yield State.goLeft state
                        yield State.goRight state
                    yield State.goStraight state
            ]
        nextStates
        |> List.where(fun s ->
            if s.StraitTiles > 10 then false
            elif target s && s.StraitTiles < 4 then false
            else true
        )
        |> List.choose (fun s -> s.Map |> Array2D.tryAtPoint s.Position |> Option.map(fun weight -> s, weight))
    
    let initialState = { Position = Point(0,0); Map = input; Direction = Point.Zero; StraitTiles = 0 }
    let settings = { VisitedKey = fun s -> s.Position,s.Direction,s.StraitTiles }
    let (Found(path)) = findPath settings { Adjacency = adj } initialState target
    path.Head.Len
