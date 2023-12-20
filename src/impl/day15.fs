module impl.day15

open Farkle
open Farkle.Builder
open Farkle.Builder.Regex

let hash s =
    s
    |> Seq.fold (fun acc c ->
        (int c + acc) * 17 % 256
    ) 0
    
module Part1 = 
    let parse (input : string) =
        input.Trim().Split(',')
    
    let solve (input : string[]) =
        input
        |> Seq.sumBy hash
    
module Part2 =
    
    type Lens = string * int
    type Operation = | Add of Lens | Remove of string
    
    let private number = Terminals.int "Number"
    let private label =
        chars PredefinedSets.AllLetters |> atLeast 1
        |> terminal "Label" (T(fun _ chars -> new string(chars)))
    let private operation = "Operation" ||= [
        !@ label .>> "=" .>>. number => fun l length -> Add(l, length)
        !@ label .>> "-" => Remove
    ]
    let parse (input : string) =
        let parser = RuntimeFarkle.build operation
        input.Split(',')
        |> Array.map (RuntimeFarkle.parseUnsafe parser)
    let execute map op =
        match op with
        | Remove label ->
            Map.change (hash label) (Option.map (Array.where (fst >> (<>)label))) map
        | Add (label, length) ->
            Map.change (hash label) (
                function
                | None -> Some [| label, length |]
                | Some box ->
                    (
                        Array.map (fun x -> if fst x = label then label, length else x) box,
                        [| label, length |]
                    )
                    ||> Array.append
                    |> Array.distinctBy fst
                    |> Some
            ) map
            
    let solve (input : Operation[]) =
        let map = Map.empty 
    
        input
        |> Array.fold execute map
        |> Map.toSeq
        |> Seq.collect (fun (box, lists) -> lists |> Seq.mapi (fun i x -> box, i+1, snd x) )
        |> Seq.map (fun (box, slot, length) -> (box + 1) * slot * length)
        |> Seq.sum
    