module impl.day24

open Farkle
open Farkle.Builder

type Ray = { x: int64; y: int64; z: int64; dx: int64; dy: int64; dz: int64 }

let private coordinate = Terminals.int64 "Coordinate"
let private ray = "Ray" ||= [
    !@ coordinate .>> "," .>>. coordinate .>> "," .>>. coordinate
        .>> "@"
        .>>. coordinate .>> ",".>>. coordinate .>> ",".>>. coordinate
        => fun x y z dx dy dz -> { x = x; y = y; z = z; dx = dx; dy = dy; dz = dz }
]

let parse input : Ray[] =
    Pattern1.read (RuntimeFarkle.parseUnsafe (RuntimeFarkle.build ray)) input
    
