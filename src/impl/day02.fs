module impl.day02

open Farkle
open Farkle.Builder
open Farkle.Builder.Regex

type CubeSet =
    { Red: int; Green: int; Blue: int }
  
type CubeSets = CubeSet[]
type Game = { Id : int; Cubes : CubeSets }

let private color =
   chars Letter 
   |> atLeast 1 
   |> terminal "Color" (T(fun _ x -> x.ToString()))
     
let private number = Terminals.int "Number"
let private cubeCount = number

// 5 red, 6 blue; 5 red, 6 blue
let private cubeSets = (
    sepBy1 (literal ";")
        // 5 red, 6 blue
        ("CubeSet" ||= [
            !@ (sepBy1 (literal ",") (
                // 5 red
                // 6 blue
                "OneColorCubes" ||= [
                !@ cubeCount .>>. color => (fun n c -> (c, n))
                ])) => (
                    Map.ofList >>
                    fun map -> {
                        Red = map |> Map.tryFind "red" |> Option.defaultValue 0
                        Blue = map |> Map.tryFind "blue" |> Option.defaultValue 0
                        Green = map |> Map.tryFind "green" |> Option.defaultValue 0
                    }
            )
        ])
)

// Game1: 5 red, 6 blue; 5 red, 6 blue
let private game = "Game" ||= [
    !& "Game" .>>. number .>> ":" .>>. cubeSets
        => fun id sets -> { Id = id; Cubes = sets |> List.toArray }
]

let parse input =
    let parser = RuntimeFarkle.build game
    input
    |> Pattern1.read (RuntimeFarkle.parseString parser)
    |> Array.map Result.get

let solve1 =
    fun input ->
        let limit = {Red = 12; Green = 13; Blue = 14 }
        input
        |> Array.filter (
            fun game ->
                game.Cubes |> Array.forall (fun set -> set.Red <= limit.Red && set.Green <= limit.Green && set.Blue <= limit.Blue)
        )
        |> Array.sumBy _.Id
    
let solve2 =
    fun input ->
        input
        |> Array.map (
            fun game ->
                 {
                    Red = game.Cubes |> Seq.map _.Red |> Seq.max
                    Green = game.Cubes |> Seq.map _.Green |> Seq.max
                    Blue = game.Cubes |> Seq.map _.Blue |> Seq.max 
                 }
        ) |> Array.map (fun set -> set.Red * set.Green * set.Blue)
        |> Array.sum