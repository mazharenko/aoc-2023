module impl.day20

open Farkle
open Farkle.Builder
open Farkle.Builder.Regex

type Pulse = | Low | High
type FlipFlop = { State: bool }
type Conjunction = { Inputs: Map<string, Pulse> }
type ModuleType =
    | Broadcast
    | FlipFlop of FlipFlop
    | Conjunction of Conjunction

type Module = { Name: string; To: string list; Type: ModuleType; }

let parse input =
    let moduleName = 
        regexString "[a-z]+" |> terminal "name" (T(fun _ s -> new string(s)))
    let moduleHeader = "header" ||= [
        !& "%" .>>. moduleName => fun name ->  FlipFlop { State = false }, name
        !& "&" .>>. moduleName => fun name -> Conjunction { Inputs = Map.empty }, name
        !& "broadcaster" =% (Broadcast, "broadcaster")
    ]
    let ``module`` = "module" ||= [
        !@ moduleHeader .>> "->" .>>. (sepBy1 (literal ",") moduleName)
            => fun (t, name) toList -> { Name = name; Type = t; To = toList }
    ]
    let parser = RuntimeFarkle.build ``module``
    let modules = 
        input
        |> Pattern1.read (RuntimeFarkle.parseUnsafe parser)
        |> Seq.map (fun m -> m.Name, m)
        |> Map.ofSeq
    modules
    |> Map.map (fun name m ->
        match m.Type with
        | Conjunction _ ->
            {
              m with Type = 
                        Conjunction {
                            Inputs = modules
                            |> Map.filter (fun _ m -> m.To |> List.contains name)
                            |> Map.map (fun _ _ -> Low) 
                        }
            }
        | _ -> m
    )
    
    
    
type private State = { Modules: Map<string, Module>; Pulses: (string * string * Pulse) list; HighSentTo: string list; LowSentTo: string list }

module private State =
    let queuePulses pulses state =
        let high, low = List.partition (fun (_, _, pulse) -> pulse = High) pulses
        {
            Modules = state.Modules
            Pulses = state.Pulses @ pulses
            HighSentTo = (high |> List.map (fun (_, x, _) -> x)) @ state.HighSentTo 
            LowSentTo = (low |> List.map (fun (_, x, _) -> x)) @ state.LowSentTo 
        }
    let queuePulsesTo source destinations pulse =
        queuePulses (destinations |> List.map(fun dest -> dest, source, pulse))
    let wasHighSentTo ``module`` state =
        state.HighSentTo |> List.contains ``module``
    
let rec private run state =
    match state.Pulses with
    | [] -> state
    | (currentModule, moduleFrom, pulse)::ps ->
        match state.Modules |> Map.tryFind currentModule with
        | None -> run { state with Pulses = ps }
        | Some targetModule -> 
            match targetModule.Type, pulse with
            | Broadcast, _ ->
                { state with Pulses = ps }
                |> State.queuePulsesTo currentModule targetModule.To pulse
                |> run
            | FlipFlop _, High -> run { state with Pulses = ps }
            | FlipFlop { State = flipFlopState }, Low ->
                {
                    state with Modules =  Map.add currentModule { targetModule with Type = FlipFlop { State = not flipFlopState }} state.Modules
                               Pulses = ps 
                } |> State.queuePulsesTo currentModule targetModule.To (if not flipFlopState then High else Low)
                |> run
            | Conjunction { Inputs = inputs }, _ ->
                let newInputsCache = inputs |> Map.add moduleFrom pulse 
                let allHigh = newInputsCache |> Map.forall (fun _ value -> value = High)
                {
                    state with Modules = state.Modules |> Map.add currentModule { targetModule with Type = Conjunction { Inputs = newInputsCache }} 
                               Pulses = ps
                } |> State.queuePulsesTo currentModule targetModule.To (if allHigh then Low else High)
                |> run
let buttonPulse = "broadcaster", "button", Low

let solve1 input =
    let initialState = { Modules = input; Pulses = []; HighSentTo = []; LowSentTo = [] }
    let {HighSentTo = highs; LowSentTo = lows} =
        seq {1..1000}
        |> Seq.fold (fun state _ -> state |> State.queuePulses [buttonPulse] |> run ) initialState
    
    highs.Length * lows.Length
    
let solve2 input =
    let initialState = { Modules = input; Pulses = []; HighSentTo = []; LowSentTo = [] }
    // rx receives Low when all rxSources receive High
    // assume each of them does that every nth button press, and n is relatively small
    let toRx = input |> Map.findKey (fun _ value -> value.To |> List.contains "rx")
    let rxSources =
        input |> Map.toSeq |> Seq.filter (fun (_, value) -> value.To |> List.contains toRx) |> Seq.map fst
        |> Seq.toArray
    Seq.initInfinite ((+)1)
    |> Seq.scan (fun (_,state) i ->
        let newState = {state with HighSentTo = []; LowSentTo = []} |> State.queuePulses [buttonPulse] |> run
        i, newState
    ) (0, initialState)
    |> Seq.scan (fun m (i, s) ->
        rxSources
        |> Seq.fold (fun m source ->
            if not <| Map.containsKey source m && State.wasHighSentTo source s
            then m |> Map.add source i
            else m
        ) m
    ) Map.empty
    |> Seq.filter (Map.count >> (=)4)
    |> Seq.head
    |> Map.values
    |> Seq.map int64
    |> Seq.reduce lcm