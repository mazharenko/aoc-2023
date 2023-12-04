module impl.day03

open System
open Microsoft.FSharp.Collections

let parse input =
    Pattern1.read (_.ToCharArray()) input |> array2D

let private isDigit c = Char.IsDigit(c)
let private isSymbol c = c <> '.' && not <| isDigit c

type private FoundNumber = { Number: int; Position: int*int; Adjacent: ((int*int)*char)[] }

/// if the array has a number (its first digit) at the specified indexes, returns it along with info about its adjacent symbols.
/// otherwise, returns None.
let private getNumberAt (i: int) (j: int) (c: char) (m: char[,]) =
    if not <| Char.IsDigit(c) then
        None
    else
        // if the previous symbol is a digit, it must have been already processed
        let previousIsDigit =
            let previous = m |> Array2D.tryGet i (j - 1) |> Option.map Char.IsDigit
            previous = Some true
        if previousIsDigit then
            None
        else
            // the whole string from the current point
            let rest = m[i, j..]
            // take digits, build a number
            let numberString = rest |> Seq.takeWhile isDigit |> Seq.toArray |> String.fromChars
            
            let adj = [|
                for adji in i-1..i+1 do
                    for adjj in j-1..j+(String.length numberString - 1)+1 do
                        match m |> Array2D.tryGet adji adjj with
                        | Some adjc when isSymbol adjc -> yield ((adji, adjj), adjc)
                        | _ -> ()
            |]
            
            { Number = int numberString; Position = i,j; Adjacent = adj } |> Some

let solve1 matrix =
    matrix
    |> Array2D.indexed |> Array2D.toSeq
    |> Seq.choose (fun ((i, j), c) -> getNumberAt i j c matrix)
    |> Seq.filter (fun number -> number.Adjacent |> Array.isEmpty |> not)
    |> Seq.map (_.Number)
    |> Seq.sum
    
type private FoundSymbol = { Symbol: char; Position: int*int; Numbers: int[] } 

let solve2 matrix =
    matrix
    |> Array2D.indexed |> Array2D.toSeq
    |> Seq.choose (fun ((i, j), c) -> getNumberAt i j c matrix)
    // reverse these "dictionary"
    |> Seq.map (fun number -> {number with Adjacent = number.Adjacent |> Array.filter (snd >> (=)'*') })
    |> Seq.collect (fun number -> number.Adjacent |> Seq.map (fun (ij, _) -> ij, number.Number))
    |> Seq.group fst (Seq.map snd)
    |> Seq.filter (fun (_, numbers) -> numbers |> Seq.length = 2)
    |> Seq.map (fun (_, numbers) -> numbers |> Seq.reduce (*))
    |> Seq.sum
