module impl.day21

#nowarn "25"

open Bfs.common
open Bfs.Custom

let parse input =
    Pattern1.read Seq.toArray input
    |> array2D
    
type State = { I: int; J:int; M: char[,]; Steps: int }
let solve1 input =
    let target = fun _ -> false 
    let settings = { VisitedKey = fun s -> s.I,s.J,s.Steps }
    let adjacency { I = i; J = j; M = m; Steps = steps } =
        if (steps >= 64) then []
        else
            Array2D.Adj.indexesValues4 m (i,j)
            |> List.where (fun (_, c) -> c = '.' || c = 'S')
            |> List.map (fun ((i, j), _) -> { I = i; J = j; M = m; Steps = steps+1 })
    let initialState = { I = 65; J = 65; M = input; Steps = 0 }
    let (NotFound(paths)) = findPath settings { Adjacency = adjacency } initialState target
    paths
    |> List.where (List.length >> ((=)65))
    |> List.length
    
let solve2 input =
    let target = fun _ -> false 
    let settings = { VisitedKey = fun s -> s.I, s.J, s.Steps%2 }
    let adjacency { I = currentI; J = currentJ; M = m; Steps = steps } =
        if steps >= 800 then []
        else
            Array2D.Adj.d4
            |> Seq.map (fun (di, dj) -> (currentI + di, currentJ + dj))
            |> Seq.where (fun (i,j) ->
                let atij = Array2D.get m (i %% Array2D.length1 m) (j %% Array2D.length2 m)
                atij = '.' || atij = 'S'
            )
            |> Seq.map (fun (i, j) -> { I = i; J = j; M = m; Steps = steps+1 })
            |> Seq.toList
    let initialState = { I = 65; J = 65; M = input; Steps = 0 }
    let (NotFound(paths)) = findPath settings { Adjacency = adjacency } initialState target
    let pathSteps = paths |> Seq.map (List.head >> _.Len >> id ) |> Seq.toArray
    seq { 65 .. 131 .. 1000 }
    |> Seq.map (fun x ->
        pathSteps |> Seq.where (fun p -> p <= x && x % 2 = p % 2) |> Seq.length
    )
    |> Seq.map int64
    |> Seq.indexed
    |> Seq.windowed 4
    // try to recognize the beginning of a quadratic sequence
    |> Seq.choose (fun ([|_,x1; _,x2; _,x3; i,x4|]) ->
        if (x4 - x3 - (x3 - x2) = x3 - x2 - (x2 - x1))
        then
            let n = 26501365L / 131L - int64 i
            let d = x4 - x3
            let d2 = x4 - x3 - (x3 - x2)
            Some (x4 + (d + d2) * n + d2 * n * (n - 1L) / 2L)
        else 
        None
    ) |> Seq.head
        