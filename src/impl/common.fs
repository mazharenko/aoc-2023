[<AutoOpen>]
module common

open System
open System.Collections.Generic
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
    
let memoize f keyf =
    let cache = Dictionary<_, _>()
    fun x ->
        let key = keyf x
        match cache.TryGetValue(key) with
        | true, value -> 
            value
        | _ ->
            let value = f x
            cache.Add(key, value)
            value
let memorec f keyf =
   let cache = Dictionary<_,_>()
   let rec frec x =
       let key = keyf x
       match cache.TryGetValue(key) with
       | true, value -> 
           value
       | _ ->
           let value = f frec x
           cache.Add(key, value)
           value
   in frec
 
type Point = | Point of (int * int) with
    static member (+)(Point (x1 : int, y1 : int), Point (x2 : int, y2 : int)) : Point = 
        let x = x1 + x2
        let y = y1 + y2
        Point (x, y)
    static member (-)(Point (x1 : int, y1 : int), Point (x2 : int, y2 : int)) : Point = 
        let x = x1 - x2
        let y = y1 - y2
        Point (x, y)
    static member (*)(Point (x1 : int, y1 : int), n:int) : Point = 
        let x = x1 * n
        let y = y1 * n
        Point (x, y)
module Point = 
    let dir (Point (x,  y)) = 
        Point (sign x, sign y)
    let mlen (Point (x1,y1)) (Point (x2, y2)) = 
        abs (x1 - x2) + abs (y1 - y2)

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
    let toJagged (source: 'T[,]) = 
        [|
            for i in (Array2D.base1 source)..(Array2D.length1 source - 1) do
                yield [|
                    for j in (Array2D.base2 source)..(Array2D.length2 source - 1) do
                        yield source.[i,j]
            |]
        |]
    let tryGet i j source =
        if (i >= Array2D.base1 source 
            && j >= Array2D.base2 source 
            && i < Array2D.length1 source + Array2D.base1 source 
            && j < Array2D.length2 source + Array2D.base2 source)
        then Some source.[i,j]
        else None
    let atPoint (Point(i,j)) source = 
        Array2D.get source i j
    let tryAtPoint (Point(i,j)) source = 
        tryGet i j source
    let transpose (a:'a[,]) = 
        Array2D.initBased
            (Array2D.base2 a)
            (Array2D.base1 a)
            (Array2D.length2 a)
            (Array2D.length1 a)
            (fun i j -> a[j, i])
    let indexed (a:'a[,]) : ((int*int)*'a)[,] = 
        Array2D.mapi (fun i j x -> (i,j),x) a
    let pointed (a:'a[,]) : seq<Point*'a> =
        a |> Array2D.mapi (fun i j x -> Point(i,j), x)
        |> toSeq
    let rows (a: 'a[,]) : int[] =
        [|
            for i in (Array2D.base1 a)..(Array2D.length1 a - 1) do
                yield i
        |]
    let cols (a: 'a[,]) : int[] =
        [|
            for j in (Array2D.base2 a)..(Array2D.length2 a - 1) do
                yield j
        |]
        
    module Adj =
        let d4 =
            [-1,0;0,1;1,0;0,-1]
        let indexesValues4 matrix (i, j) =
            d4 |> List.choose (fun (di, dj) ->
                let adji = i + di
                let adjj = j + dj
                match tryGet adji adjj matrix with
                | Some adj -> Some ((adji,adjj),adj)
                | None -> None
            )
        let indexes4 matrix (i,j) =
            indexesValues4 matrix (i,j)
            |> List.map fst
        
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
 