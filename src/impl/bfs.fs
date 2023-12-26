[<AutoOpen>]
module Bfs

#nowarn "25"

open System.Collections.Generic

[<AutoOpen>]
module common =
    type PathItem<'a> = { Item: 'a; Len: int }
    type Path<'a> = PathItem<'a> list
    type Result<'a> = 
        | Found of Path<'a>
        | NotFound
    
module Custom = 
    type Adjacency<'a> = 'a -> 'a list
    
    type Target<'a> = 'a -> bool
    module Targets =
        let value x : Target<_> = (fun x' -> x = x')

    type Settings<'a,'key> = { VisitedKey: 'a -> 'key; }
    module Settings =
        let defaults : Settings<_,_> = {VisitedKey = id;}
    
    type Parameters<'a> = { Adjacency: Adjacency<'a> }

    
    // attempt to introduce something more generic than searching in a graph
    // more like traversal. fold seems to suit the most scenarios.
    // can't imagine mapping or unfolding a graph.
    // scan could be useful though
    type TraversalResult = | Continue | Interrupt
    type Folder<'a, 'res> = 'res -> Path<'a> -> 'res * TraversalResult
    let rec private fold'
        (folder : Folder<'a, 'res>)
        (folderState : 'res)
        (visited: HashSet<'key>) 
        (q : Queue<PathItem<'a> list>)
        (settings : Settings<'a, 'key>)
        (parameters : Parameters<'a>) =
        if q.Count = 0 then folderState
        else
            let current::rest = q.Dequeue()
            match folder folderState (current::rest) with
            | x, Interrupt -> x
            | x, Continue ->
                let adjacent = parameters.Adjacency current.Item
                adjacent
                |> Seq.filter (fun a ->
                        let key = settings.VisitedKey a
                        not <| visited.Contains key
                    )
                |> Seq.iter (
                    fun value' -> 
                        visited.Add (settings.VisitedKey value') |> ignore
                        q.Enqueue ({Item = value'; Len = current.Len+1}::current::rest)
                )
                fold' folder x visited q settings parameters
        
    let fold (settings : Settings<'a,'key>) (parameters : Parameters<'a>) (start: 'a) folder initialState =
        let visited = HashSet<'key>()
        visited.Add (settings.VisitedKey start) |> ignore
        let queue = Queue<Path<'a>>()
        queue.Enqueue([{Item = start; Len = 0}])
        fold' folder initialState visited queue settings parameters

    let findPath (settings : Settings<'a,'key>) (parameters : Parameters<'a>) (start: 'a) (target : Target<'a>) = 
        let folder : Folder<'a, Result<'a>> =
            fun _ (current::_ as path) -> 
                if target current.Item then (Found path), Interrupt
                else NotFound, Continue
        fold settings parameters start folder NotFound
        
        
module Matrix = 
    type State<'a> = { Coordinates: int*int; Value: 'a; Matrix: 'a[,] }

    type Adjacency<'a> = int*int -> 'a -> 'a[,] -> ((int*int)*'a) list
    module Adjacencies = 
        let A4 : Adjacency<_> = 
            fun (i,j) _ m ->
                [
                    (i-1, j)
                    (i+1, j)
                    (i, j-1)
                    (i, j+1)
                ] |> List.choose (fun (i',j') -> 
                    if (i' >= Array2D.base1 m 
                        && j' >= Array2D.base2 m 
                        && i' < Array2D.length1 m 
                        && j' < Array2D.length2 m)
                    then Some ((i',j'), m[i', j'])
                    else None
                )
        let where condition (adj: Adjacency<'a>) : Adjacency<'a> =
            fun (i,j) value m ->
                adj (i,j) value m
                |> List.filter(fun (_, value') -> condition value value')

    type Target<'a> = int*int -> 'a -> bool
    module Targets =
        let value x : Target<_> 
            = fun _ x' -> x' = x
        let at i j : Target<_>
            = fun (i', j') _ -> i = i' && j = j'

    type Parameters<'a> = { Adjacency: Adjacency<'a> }
    module Parameters = 
        let common = 
            {
                Adjacency = Adjacencies.A4
            }

    let private createSettings<'a>() : Custom.Settings<State<'a>,int*int> =
        {
            VisitedKey = _.Coordinates
        }

    let private convertParameters parameters : Custom.Parameters<State<_>> = 
        {
            Adjacency = 
                fun state -> 
                    parameters.Adjacency state.Coordinates state.Value state.Matrix
                    |> List.map (fun (c, v) -> { Coordinates = c; Value = v; Matrix = state.Matrix})
        }

    let findPath (parameters : Parameters<'a>) (matrix: 'a[,]) (starti,startj as start: int*int) (target : Target<'a>) = 
        let customSettings = createSettings<'a>()
        let customParameters = convertParameters parameters
        let customTarget : Custom.Target<State<'a>> =
            fun state -> target state.Coordinates state.Value
        let result = Custom.findPath customSettings customParameters {Coordinates = start; Value = matrix[starti, startj]; Matrix = matrix} customTarget
        result
