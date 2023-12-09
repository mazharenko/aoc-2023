module impl.day09

let parse input =
    input
    |> Pattern1.read (fun line -> line.Split ' ' |> Array.map int)
    
let private extrapolated history =
   history
   |> Array.unfold (fun h ->
       if h |> Seq.forall ((=)0) then None
       else
           let newHistory =
               h |> Seq.pairwise |> Seq.map (fun (x1, x2) -> x2 - x1)
               |> Array.ofSeq
           Some (h, newHistory)
   )
    
let solve1 input =
    input
    |> Array.sumBy (fun h -> extrapolated h |> Array.sumBy Array.last)
    
let solve2 input =
    input
    |> Array.map extrapolated
    |> Array.sumBy (fun h ->
        h |> Seq.map Seq.head |> Seq.reduceBack (-))