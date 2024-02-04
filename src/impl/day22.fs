module impl.day22

open Farkle
open Farkle.Builder

type Point3 = int*int*int
module private Point3 =
    let x (x',_,_) = x'
    let y (_,y',_) = y'
    let z (_,_,z') = z'
type Brick = { From: Point3; To: Point3 }
module private Brick =
    let allXY brick =
        Seq.allPairs
            (seq {Point3.x brick.From .. Point3.x brick.To})
            (seq {Point3.y brick.From .. Point3.y brick.To})

let private number = Terminals.int "Number"
let private point = "Point" ||= [
    !@ number .>> "," .>>. number .>> "," .>>. number
        => fun x y z -> x,y,z
]
let private brick = "Brick" ||= [
    !@ point .>> "~" .>>. point
        => fun p1 p2 -> { From = p1; To = p2 }
]

let parse input =
    Pattern1.read (RuntimeFarkle.parseUnsafe (RuntimeFarkle.build brick)) input

type private State = {
    Bricks: Brick list
    Top: Map<int*int, int*Brick>
    Supports: Map<Brick, Set<Brick>>
    SupportedBy: Map<Brick, Set<Brick>>
}
module private State =
    let fromBricks bricks =
        {
            Bricks = bricks |> Seq.sortBy (_.From >> Point3.z) |> Seq.toList
            Top = Map.empty
            Supports = Map.empty
            SupportedBy = Map.empty 
        }
    
let rec private fall state =
    match state.Bricks with
    | [] -> state
    | brick::rest ->
        let toRestBricks, toRestZ =
            Brick.allXY brick
            |> Seq.fold (fun (foundPointsToRest,z) p ->
                let existing = Map.tryFind p state.Top
                match existing with
                | None -> foundPointsToRest,z
                | Some (existingZ,brick) ->
                    if existingZ > z then (Set.add brick Set.empty),existingZ
                    elif existingZ = z then (Set.add brick foundPointsToRest),existingZ
                    else foundPointsToRest,z
            ) (Set.empty,0)
        let newTop = 
            Brick.allXY brick
            |> Seq.fold (fun top xy -> Map.add xy (toRestZ + Point3.z brick.To - Point3.z brick.From + 1, brick) top) state.Top
        {
            Bricks = rest
            Top = newTop
            SupportedBy =
                state.SupportedBy |> Map.add brick toRestBricks 
            Supports =
                toRestBricks |> Seq.fold (
                    fun supports b ->
                        supports
                        |> Map.change b (
                            function
                            | None -> Set.ofList [brick] |> Some
                            | Some bb -> Set.add brick bb |> Some
                        )
                ) state.Supports
        } |> fall
        
let solve1 input =
    let {Supports = supports; SupportedBy = supportedBy} =
        input |> State.fromBricks |> fall
    input
    |> Seq.where (
        fun brick ->
            match Map.tryFind brick supports with
            | None -> true
            | Some supported ->
                supported |> Seq.forall (fun sup -> supportedBy[sup] |> Set.count > 1)
    )
    |> Seq.length
    
let rec private wouldFall supports supportedBy falling brick =
    match supports |> Map.tryFind brick with
    | None -> falling
    | Some supported ->
        let newFalling =
            supported
            |> Set.filter (fun s -> supportedBy |> Map.find s |> Seq.forall(fun x -> x = brick || Set.contains x falling))
        newFalling
        |> Seq.fold (wouldFall supports supportedBy) (Set.union falling newFalling)

let solve2 input =
    let {Supports = supports; SupportedBy = supportedBy} =
        input |> State.fromBricks |> fall
    input
    |> Seq.map (fun brick -> wouldFall supports supportedBy Set.empty brick)
    |> Seq.sumBy Seq.length