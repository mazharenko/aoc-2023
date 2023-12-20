module impl.day18

open Farkle
open Farkle.Builder
open Farkle.Builder.Regex

let private down = Point (1L,0L)
let private up = Point (-1L,0L)
let private right = Point (0L,1L)
let private left = Point (0L,-1L)

let private direction = "Direction" ||= [
    !& "R" =% right
    !& "D" =% down
    !& "L" =% left
    !& "U" =% up
]

let private steps = Terminals.int "Steps"

module Part1 =
    let private digInstruction = "Dig" ||= [
        !@ direction .>>. steps .>> "(" .>> (any |> atLeast 1 |> terminalU "")
            => fun dir steps -> dir * int64 steps
    ]
    let parse input =
        let parser = RuntimeFarkle.build digInstruction
        input
        |> Pattern1.read (RuntimeFarkle.parseUnsafe parser)
     

module Part2 = 
    let parse input =
        let digInstruction = "Dig" ||= [
            !@ direction .>>. steps .>> "(" .>>. (regexString "[#][0-9a-f]{6}" |> terminal "Color" (T(fun _ c -> new string(c)))) .>> ")"
            => fun _ _ color ->
                let dir = match color[^0] with | '0' -> right | '1' -> down | '2' -> left | '3' -> up
                let steps = System.Convert.ToInt32(color[1..^1], 16)
                dir * int64 steps
        ]
        let parser = RuntimeFarkle.build digInstruction
        input
        |> Pattern1.read (RuntimeFarkle.parseUnsafe parser)
   
let solve input =
    let points =
        input
        |> Array.scan (+) (Point(0L,0L))
    let p = input |> Array.map (fun p -> int64 <| abs (Point.x p + Point.y p)) |> Array.sum
    
    // shoelace formula
    let area = 
        abs (
            Array.pairwise points
            |> Array.sumBy (fun (p1, p2) -> (Point.x p1) * (Point.y p2) - (Point.y p1) * (Point.x p2))
        ) / 2L
    // Pick's theorem
    area - p/2L + 1L + p
    