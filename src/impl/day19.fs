module impl.day19

#nowarn "25"

open Farkle
open Farkle.Builder
open Farkle.Builder.Regex

type Rating = string 
type Part = Map<string, int64>
type WfName = string
type RuleVerdict = | Accept | Reject | RedirectTo of WfName
type RuleCondition = { Rating: Rating; Op: string; Operand: int64 } 
type Rule = {Condition: RuleCondition; Verdict: RuleVerdict}
type Wf = { Name: WfName; Rules: Rule list;  }

module private Parsing = 
    let number = Terminals.int64 "number"
    let rating = chars "xmas" |> terminal "rating" (T(fun _ s -> new string(s)))
    let condition = "condition" ||= [
        !@ rating .>> "<" .>>. number => fun r num -> { Rating = r; Op = "<"; Operand = num }
        !@ rating .>> ">" .>>. number => fun r num -> { Rating = r; Op = ">"; Operand = num }
    ]
    let wfName = regexString "[a-z]+" |> terminal "wf name" (T(fun _ s -> new string(s)))
    let verdict = "verdict" ||= [
        !@ wfName => RedirectTo;
        !& "A" =% Accept
        !& "R" =% Reject
    ]
    let rule = "rule" ||= [
        !@ condition .>> ":" .>>. verdict => fun c v -> { Condition = c; Verdict = v }
    ]
    let wf =
        "wf" ||= [
        !@ wfName .>> "{" .>>. (many1 ("rule," ||= [ !@ rule .>> "," |> asIs ]))
            .>>. verdict .>> "}" => fun name rules fallback ->
                // universal rule expected to match any point / range
                let fallbackRule = [{Condition = {Rating = "x"; Op = ">"; Operand = 0 }; Verdict = fallback }]
                {Name = name; Rules = rules @ fallbackRule; }
        ]
    let partRating = "partRating" ||= [
        !@ rating .>> "=" .>>. number => fun r n -> r,n
    ]
    let part = "part" ||= [
        !& "{" .>>. (sepBy1 (literal ",") partRating) .>> "}" |> asIs
    ]
    let wfParser =
        wf
        |> DesigntimeFarkle.caseSensitive true
        
let parse input =
    let [|wfInput; partsInput|] = Pattern2.read id input
    let partParser = RuntimeFarkle.build Parsing.part
    let workFlows =
        Pattern1.read (RuntimeFarkle.parseUnsafe (RuntimeFarkle.build Parsing.wfParser)) wfInput
        |> Seq.map (fun x -> x.Name, x)
        |> Map.ofSeq
    let parts =
        Pattern1.read (RuntimeFarkle.parseUnsafe partParser) partsInput
        |> Array.map Map.ofList
    workFlows, parts
   
let countRanges (ranges: Map<Rating, Range.T list>) =
    if ranges |> Map.isEmpty then 0L
    else
        ranges
        |> Map.values
        |> Seq.map (List.sumBy Range.length)
        |> Seq.fold (*) 1L

let rec run (workflows: Map<WfName, Wf>) (workflow : Wf) (ranges: Map<Rating, Range.T list>) =
    if Map.isEmpty ranges then countRanges ranges
    else
        match workflow.Rules with
        | [] -> countRanges ranges
        | rule::rules ->
            let ratingRanges = ranges[rule.Condition.Rating]
            let matchRange = 
                match rule.Condition.Op, rule.Condition.Operand with
                | ">", than -> Range.create (than+1L) 1000000
                | "<", than -> Range.create 0 (than-1L)
            let matchingSegments = ratingRanges |> List.choose (Range.intersect matchRange)
            let notMatchingSegments = ratingRanges |> List.collect (fun l -> Range.subtract l matchRange)
            let newRangesMatching =
                if matchingSegments |> List.isEmpty then Map.empty
                else Map.add rule.Condition.Rating matchingSegments ranges
            let newRangesNotMatching =
                if notMatchingSegments |> List.isEmpty then Map.empty
                else Map.add rule.Condition.Rating notMatchingSegments ranges
            match rule.Verdict with
            | Accept ->
                run workflows { workflow with Rules = rules } newRangesNotMatching
                |> (+) (countRanges newRangesMatching)
            | Reject ->
                run workflows { workflow with Rules = rules } newRangesNotMatching
            | RedirectTo redirectTo ->
                run workflows { workflow with Rules = rules } newRangesNotMatching
                |> (+) (run workflows workflows[redirectTo] newRangesMatching)
    
// part1 implemented on one-point ranges so that part2 logic is reused
let solve1 (workflows: Map<WfName, Wf>) parts =
    parts
    |> Array.sumBy (fun part ->
        let res =
            part
            |> Map.map (fun _ part -> [Range.create part part])
            |> run workflows workflows["in"]
        if res > 0 then part.Values |> Seq.sum
        else 0
    )
    
let solve2 (workflows: Map<WfName, Wf>) =
    [
        "x", [Range.create 1 4000]
        "m", [Range.create 1 4000]
        "a", [Range.create 1 4000]
        "s", [Range.create 1 4000]
    ]
    |> Map.ofSeq
    |> run workflows workflows["in"]
  