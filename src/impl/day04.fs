module impl.day04

open Farkle
open Farkle.Builder

type Card = { Winning: Set<int>; Our: Set<int> }

let parse input =
    let number = Terminals.int "Number"
    let numbers = many1 number
    let card = "Card" ||= [
        !& "Card" .>> number .>> ":" .>>.
            numbers .>> "|" .>>. numbers
            => (fun winning our -> { Winning = Set.ofList winning; Our = Set.ofList our })
    ]
    let parser = RuntimeFarkle.build card
    let parseLine line =
        RuntimeFarkle.parseUnsafe parser line
    Pattern1.read parseLine input
    
let solve1 input =
    input
    |> Array.map (fun card -> Set.intersect card.Winning card.Our)
    |> Array.map (fun matchingNumbers -> pown 2 (Set.count matchingNumbers - 1))
    |> Array.sum
    
let rec private calculate2 (currentCount, list) =
    match list with
    | [] -> currentCount
    // don't expect to have any matching numbers in the last game
    | [(count, _)] -> count + currentCount
    // having headCount current cards,
    // determine matches,
    // take matches number of the following cards,
    // increase their number by headCount,
    // repeat
    | (headCount, head)::tail ->
        let matches = Set.intersect head.Winning head.Our |> Set.count
        let cardsToCopy, rest = List.splitAt matches tail
        let newTail =
            (cardsToCopy |> List.map(fun (count, card) -> count+headCount, card)) @ rest
        (calculate2 (currentCount + headCount, newTail))
    
let solve2 (input : Card[]) : int =
    input
    |> Array.map (fun card -> 1,card)
    |> Array.toList
    |> (fun x -> calculate2 (0, x))