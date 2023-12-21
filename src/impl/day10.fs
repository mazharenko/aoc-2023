module impl.day10

// incomplete pattern matches
#nowarn "25"

[<RequireQualifiedAccess>]
type Shape = | ``│`` | ``─`` | ``└`` | ``┘`` | ``┐`` | ``┌`` | Empty
module Shape =
    let private toNorth = Point(-1,0)
    let private toWest = Point(0,-1)
    let private toSouth = Point(1,0)
    let private toEast = Point(0,1)
    let fromChar char =
        match char with
        | '|' -> Shape.``│``
        | '-' -> Shape.``─``
        | 'L' -> Shape.``└``
        | 'J' -> Shape.``┘``
        | '7' -> Shape.``┐``
        | 'F' -> Shape.``┌``
        | '.' -> Shape.Empty
    let connectedTilesVectors shape =
        match shape with
        | Shape.``│`` -> [ toNorth; toSouth ]
        | Shape.``─`` -> [ toEast; toWest ]
        | Shape.``└`` -> [ toNorth; toEast ]
        | Shape.``┘`` -> [ toNorth; toWest ]
        | Shape.``┐`` -> [ toWest; toSouth ]
        | Shape.``┌`` -> [ toEast; toSouth ]
        | Shape.Empty -> []
    let fromConnectedDirections directions =
        let dirs = directions |> Seq.map Point.dir |> Set.ofSeq
        if (dirs = set [ toNorth; toSouth ]) then Shape.``│``
        elif (dirs = set [ toEast; toWest ]) then Shape.``─``
        elif (dirs = set [ toNorth; toEast ]) then Shape.``└``
        elif (dirs = set [ toNorth; toWest ]) then Shape.``┘``
        elif (dirs = set [ toWest; toSouth ]) then Shape.``┐``
        elif (dirs = set [ toEast; toSouth ]) then Shape.``┌``
        else invalidArg (nameof(directions)) "unknown directions"
        
type Pipe = { Shape: Shape; Start: bool; }
module Pipe =
    let createDefault shape = { Shape = shape; Start = false }
    
let private connections matrix point =
    let pipe = Array2D.atPoint point matrix
    Shape.connectedTilesVectors pipe.Shape
    |> List.choose (fun p ->
        let adjPoint = p + point
        match Array2D.tryAtPoint matrix adjPoint with
        | Some adj -> Some (adjPoint, adj)
        | None -> None
    )

let parse input =
    let matrixWithoutStartResolved =
        Pattern1.read Seq.toArray input
        |> array2D
        |> Array2D.map (
            function
            | 'S' -> { Shape = Shape.Empty; Start = true }
            | c -> c |> Shape.fromChar |> Pipe.createDefault 
        )
    matrixWithoutStartResolved
    |> Array2D.mapi (fun i j pipe ->
          let p = Point(i,j)
          match pipe with
          | { Start = true } -> 
              // determine the S-tart shape checking which neighbors connect with S
              let allAdj = Array2D.Adj.indexes4 matrixWithoutStartResolved (i, j)
              let connectedAdj =
                  allAdj
                  |> List.map Point
                  |> List.where (
                      fun adj -> connections matrixWithoutStartResolved adj
                                 |> List.contains (p, pipe)
                  ) |> List.map (fun x -> x - p)
              { pipe with Shape = Shape.fromConnectedDirections connectedAdj }
           | _ -> pipe
    )
    
let rec private findPathToStart (path : (Point*Pipe) list) matrix =
    match path with
    // not the found target but just the beginning
    | [startPoint, _ as start] ->
        let anyConnectedPoint = connections matrix startPoint |> List.head
        findPathToStart [anyConnectedPoint; start] matrix
    // found target
    | (_, { Start = true }) :: _ -> path
    // only forward
    | currentPoint,_ as current :: previous :: restPath ->
        let connected = connections matrix currentPoint
        let firstConnected = 
            connected |> List.find ((<>)previous)
        findPathToStart (firstConnected::current::previous::restPath) matrix
            
let solve1 (input: Pipe[,]) =
    let startPoint, start =
        input
        |> Array2D.indexed
        |> Array2D.toSeq |> Seq.find (fun (_, pipe) -> pipe.Start)
        
    let path = findPathToStart [Point startPoint, start] input
    let pathLength = path |> List.length
    (pathLength - 1) / 2
     
type Type = | Inside | Outside | InsideLeft | InsideRight
let solve2 input =
    let startPoint, start =
             input
             |> Array2D.indexed
             |> Array2D.toSeq |> Seq.find (fun (_, pipe) -> pipe.Start)
    let path = findPathToStart [Point startPoint, start] input
    let borderPoints = path |> Seq.map fst |> Set.ofSeq
     
    let insidePoints = 
        input |> Array2D.indexed
        |> Array2D.toJagged
        |> Array.collect(fun line ->
            line |> Array.mapFold (fun state (x, tile) ->
                if (borderPoints |> Set.contains (Point x))
                then
                    let newState =
                        match state,tile.Shape with
                        | _, Shape.``─`` -> state
                        | Inside, Shape.``│`` -> Outside
                        | Inside, Shape.``└`` -> InsideRight
                        | Inside, Shape.``┌`` -> InsideLeft
                        | Outside, Shape.``│`` -> Inside
                        | Outside, Shape.``└`` -> InsideLeft
                        | Outside, Shape.``┌`` -> InsideRight
                        | InsideLeft, Shape.``┐`` -> Inside
                        | InsideLeft, Shape.``┘`` -> Outside
                        | InsideRight, Shape.``┐`` -> Outside
                        | InsideRight, Shape.``┘`` -> Inside
                    None, newState
                else
                    if state = Inside then Some x, state
                    else None, state
            ) Outside
            |> fst
            |> Array.choose id
        )
        
    insidePoints |> Array.length
