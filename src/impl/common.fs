[<AutoOpen>]
module common

open System
open System.Collections.Generic
open System.Numerics
open Farkle

let splitToTuple2 (separators : string array) (s : string) =
    let split = s.Split(separators, StringSplitOptions.RemoveEmptyEntries)
    split[0], split[1]
    
let (|Eq|_|) x y =
    if (x = y) then Some()
    else None

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

let inline (%%) (x: ^a when INumber<^a>) (y: ^a) : ^a = ((x % y) + y) % y

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

module gmath =
    let sign (a: 'a when INumber<'a>) =
        
        a |> 'a.Sign |> 'a.CreateChecked
    let abs (a: 'a when INumberBase<'a>) =
        'a.Abs a
 
type Point<^a when INumber<^a>> = | Point of (^a * ^a) with
    static member inline (+)(Point (x1 : ^a, y1 : ^a), Point (x2 : ^a, y2 : ^a)) : Point<^a> = 
        let x = x1 + x2
        let y = y1 + y2
        Point (x, y)
    static member inline (-)(Point (x1 : ^a, y1 : ^a), Point (x2 : ^a, y2 : ^a)) : Point<^a> = 
        let x = x1 - x2
        let y = y1 - y2
        Point (x, y)
    static member inline (*)(Point (x1 : ^a, y1 : ^a), n:^a) : Point<^a> = 
        let x = x1 * n
        let y = y1 * n
        Point (x, y)
    static member Zero = Point('a.Zero, 'a.Zero)
    
type Point = Point<int>
    
module Point =
    let create x y = Point(x, y)
    let x (Point(xx, _)) = xx
    let y (Point(_, yy)) = yy
    let dir (Point(x: 'a, y: 'a)) : Point<'a> =
        Point(gmath.sign x, gmath.sign y)
    let mlen (Point(x1: 'a, y1: 'a)) (Point (x2: 'a, y2: 'a)) =
        gmath.abs (x1 - x2) + gmath.abs (y1 - y2)
    
    let down() : Point<'a> = Point ('a.One,'a.Zero)
    let up() : Point<'a> = Point ('a.op_CheckedUnaryNegation 'a.One, 'a.Zero)
    let right() : Point<'a> = Point ('a.Zero,'a.One)
    let left() : Point<'a> = Point ('a.Zero, 'a.op_CheckedUnaryNegation 'a.One)
    
    module Rotation =
        type Rotation = private | T of int[,]
        with static member (*) (T(r1), T(r2)) =
                Array2D.init 2 2 (fun i j ->
                    (r1[i,*], r2[*,j])
                    ||> Seq.zip
                    |> Seq.map (fun (x,y) -> x*y)
                    |> Seq.sum
                ) |> T
            
        let id : Rotation =
            [
                [1; 0]
                [0; 1]
            ]
            |> array2D |> T
        let cw : Rotation =
            [
                [0; 1]
                [-1; 0]
            ] |> array2D |> T
        let ccw : Rotation =
            [
                [0; -1]
                [1; 0]
            ] |> array2D |> T
        
        let rotate (T(rotation):Rotation) (point:Point) =
            let i =
                rotation[0,0] * x point
                + rotation[0,1] * y point
            let j =
                rotation[1,0] * x point
                + rotation[1,1] * y point
            Point(i,j)
        let rotateCW = rotate cw
        let rotateCCW = rotate ccw

module Pattern1 =
    let read (f : string -> 'a) (data : string) = 
        data.Split([|"\n"; "\r"|], StringSplitOptions.RemoveEmptyEntries) 
        |> Array.map f

module Pattern2 = 
    let read (f : string -> 'a) (data : string) = 
        data.Split([|"\n\n"; "\r\n\r\n"|], StringSplitOptions.RemoveEmptyEntries) 
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
                        yield source[i,j]
            |]
        |]
    let tryGet i j source =
        if (i >= Array2D.base1 source 
            && j >= Array2D.base2 source 
            && i < Array2D.length1 source + Array2D.base1 source 
            && j < Array2D.length2 source + Array2D.base2 source)
        then Some source[i,j]
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
    let pointed (a:'a[,]) : seq<Point<int>*'a> =
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
    let rotateCCW (a: 'a[,]) : 'a[,] =
        Array2D.init (Array2D.length2 a) (Array2D.length1 a)
            (fun i j -> a[j, Array2D.length1 a - i - 1])
    let rotateCW (a: 'a[,]) : 'a[,] =
        Array2D.init (Array2D.length2 a) (Array2D.length1 a)
            (fun i j -> a[Array2D.length2 a - j - 1, i ])
    let findIndex predicate a =
        a |> indexed
        |> toSeq
        |> Seq.find (snd >> predicate)
        |> fst
        
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
        
module RuntimeFarkle =
    let parseUnsafe parser s =
        RuntimeFarkle.parseString parser s
        |> Result.get
        
module Range =
    type T = int64*int64
    let create from to' : T = (from,to')
    let start ((from, _) : T) = from
    let finish ((_, to') : T) = to'
    let isEmpty ((from, to') : T) =
        to' < from
    let length ((from, to') : T) =
        let len = (to' - from) + 1L
        if (len >= 0) then len else 0
    let contains x ((from, to') : T) = from <= x && x <= to'
    let intersect (r1: T) (r2: T) : T option=
        let intersectionCandidate =
            (max (start r1) (start r2), min (finish r1) (finish r2))
        if isEmpty intersectionCandidate then None
        else Some intersectionCandidate
    let subtract ((from1, to1) : T) ((from2, to2) : T) : T list=
        [
            (from1, min (from2-1L) to1)
            (max from1 (to2+1L), to1)
        ] |> List.filter (not << isEmpty)
        
    let shift n ((from, to') : T) =
        (from + n, to' + n)