module impl.day11

let parse input =
    Pattern1.read Seq.toArray input
    |> array2D
    
let private calculate emptyTileSize input =
    let galaxyPoints =
        input
        |> Array2D.pointed
        |> Seq.where (snd >> (=)'#')
        |> Seq.map fst
    let emptyRows =
        [
            for i in 0 .. Array2D.length1 input - 1 do
                if (input[i,*] |> Seq.forall ((=)'.'))
                then yield i
        ]
    let emptyCols =
        [
            for j in 0 .. Array2D.length2 input - 1 do
                if (input[*,j] |> Seq.forall ((=)'.'))
                then yield j
        ]
    let expandedGalaxyPoints =
        galaxyPoints |> Seq.map (
            fun (Point (x, y) as g) ->
                let emptyRowCount = emptyRows |> Seq.where (fun row -> row < x) |> Seq.length
                let emptyColCount = emptyCols |> Seq.where (fun col -> col < y) |> Seq.length
                g + Point(emptyRowCount, emptyColCount) * (emptyTileSize - 1) 
        )

    (expandedGalaxyPoints, expandedGalaxyPoints)
    ||> Seq.allPairs
    // remove duplicates
    |> Seq.where (fun (p1, p2) -> p1 > p2)
    |> Seq.map (fun (p1, p2) -> Point.mlen p1 p2)
    |> Seq.map int64
    |> Seq.sum
    
let solve1 input =
    calculate 2 input
    
let solve2 input =
    calculate 1000000 input