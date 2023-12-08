[<AutoOpen>]
module common

open System.Numerics

let splitToTuple2 (separators : string array) (s : string) =
    let split = s.Split(separators, System.StringSplitOptions.RemoveEmptyEntries)
    split.[0], split.[1]
    
let inline gcd (x : ^a) (y : ^a) : ^a =
    let r =
        (BigInteger.CreateChecked x, BigInteger.CreateChecked y)
        |> BigInteger.GreatestCommonDivisor
    'a.CreateChecked r
    
let inline lcm x y = 
    let zero = LanguagePrimitives.GenericZero
    if x = zero || y = zero
    then zero
    else abs (x * y) / gcd x y
    
module Pattern1 =
    let read (f : string -> 'a) (data : string) = 
        data.Split([|"\n"; "\r"|], System.StringSplitOptions.RemoveEmptyEntries) 
        |> Array.map f

module Pattern2 = 
    let read (f : string -> 'a) (data : string) = 
        data.Split([|"\n\n"; "\r\n\r\n"|], System.StringSplitOptions.RemoveEmptyEntries) 
        |> Array.map f
        
module Seq =
    let group keySelector valueSelector source = 
        source |> Seq.groupBy keySelector 
        |> Seq.map (fun (key, entries) -> key, valueSelector entries)
    
module Array2D = 
    let toSeq (a:'a[,]) : seq<'a> =
        a |> Seq.cast<'a>
        
    let tryGet i j source =
        if (i >= Array2D.base1 source 
            && j >= Array2D.base2 source 
            && i < Array2D.length1 source + Array2D.base1 source 
            && j < Array2D.length2 source + Array2D.base2 source)
        then Some source.[i,j]
        else None
    let indexed (a:'a[,]) : (((int*int)*'a)[,]) = 
        Array2D.mapi (fun i j x -> (i,j),x) a
        
module String =
    let fromChars (chars : char[]) =
        new string(chars)
        
    let indexOf (s : string) (pattern : string) =
        match s.IndexOf(pattern) with
        | -1 -> None
        | index -> Some index
        
    let lastIndexOf (s : string) (pattern : string) =
        match s.LastIndexOf(pattern) with
        | -1 -> None
        | index -> Some index

module Result =
    let get =
        function 
        | Ok x -> x
        | Error error -> failwith (error.ToString())
 