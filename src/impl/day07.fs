module impl.day07

#nowarn "25"

open System

module Part1 =
    [<CustomComparison>]
    [<CustomEquality>]
    type Hand =
        | Cards of string
        override this.Equals(other) =
            match other with
            | :? Hand as (Cards otherCards) ->
                let (Cards cards) = this
                cards = otherCards
            | _ -> false
        override this.GetHashCode() =
            let (Cards cards) = this
            cards.GetHashCode();
            
        interface IComparable with 
            member this.CompareTo other =
                (this :> IComparable<Hand>).CompareTo (other :?> Hand)
        interface IComparable<Hand> with
            member this.CompareTo (Cards otherCards) =
                let (Cards cards) = this
                let handRank cards =
                    let counts = cards |> Seq.group id Seq.length |> Seq.map snd |> Seq.sortDescending |> List.ofSeq
                    match counts with
                    | 5 :: _ -> 7
                    | 4 :: _ -> 6
                    | 3 :: 2 :: _ -> 5
                    | 3 :: _ -> 4
                    | 2 :: 2 :: _ -> 3
                    | 2 :: _ -> 2
                    | _ -> 1
                let handComparison = compare (handRank otherCards) (handRank cards)
                if (handComparison = 0)
                then 
                    let allCardsFromWeakestToStrongest = "23456789TJQKA"
                    // build array of card strengths - indexes in the string above
                    // arrays are compared properly in F#
                    let cardStrengthArray =
                        Seq.toArray >> Array.map (string >> String.indexOf allCardsFromWeakestToStrongest)
                    compare
                        (otherCards |> cardStrengthArray)
                        (cards |> cardStrengthArray)
                else handComparison

    let parse input =
        Pattern1.read (fun line ->
            let hand, bet = splitToTuple2 [| " " |] line
            Cards hand, int bet
        ) input 
            
    let solve input =
        input
        |> Array.sortByDescending fst
        |> Array.mapi (fun i x -> (i + 1) * (snd x))
        |> Array.sum

module Part2 =
    
    [<CustomComparison>]
    [<CustomEquality>]
    type Hand =
        | Cards of string
        override this.Equals(other) =
            match other with
            | :? Hand as (Cards otherCards) ->
                let (Cards cards) = this
                cards = otherCards
            | _ -> false
        override this.GetHashCode() =
            let (Cards cards) = this
            cards.GetHashCode();
            
        interface IComparable with 
            member this.CompareTo other =
                (this :> IComparable<Hand>).CompareTo (other :?> Hand)
        interface IComparable<Hand> with
            member this.CompareTo (Cards otherCards) =
                let (Cards cards) = this
                let handRank cards =
                    let jokerCounts, restCounts =
                        cards
                        |> Seq.countBy id
                        |> Seq.sortByDescending snd |> List.ofSeq |> List.partition (fst >> ((=)'J'))
                    let countsWithJokers =
                        List.map snd <|
                        match (jokerCounts, restCounts) with
                        | [], _ -> restCounts
                        | jokers, [] -> jokers
                        | [(_, jokers)], (first, firstCount)::rest -> (first, firstCount + jokers)::rest
                    
                    match countsWithJokers with
                    | 5 :: _ -> 7
                    | 4 :: _ -> 6
                    | 3 :: 2 :: _ -> 5
                    | 3 :: _ -> 4
                    | 2 :: 2 :: _ -> 3
                    | 2 :: _ -> 2
                    | _ -> 1
                let handComparison = compare (handRank otherCards) (handRank cards)
                if (handComparison = 0)
                then 
                    let allCardsFromWeakestToStrongest = "J23456789TQKA"
                    // build array of card strengths - indexes in the string above
                    // arrays are compared properly in F#
                    let cardStrengthArray =
                        Seq.toArray >> Array.map (string >> String.indexOf allCardsFromWeakestToStrongest)
                    compare
                        (otherCards |> cardStrengthArray)
                        (cards |> cardStrengthArray)
                else handComparison
                
    let parse input =
        Pattern1.read (fun line ->
            let hand, bet = splitToTuple2 [| " " |] line
            Cards hand, int bet
        ) input 

    let solve input = 
        input
        |> Array.sortByDescending fst
        |> Array.mapi (fun i x -> (i + 1) * (snd x))
        |> Array.sum