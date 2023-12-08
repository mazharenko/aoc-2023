using Microsoft.FSharp.Collections;
using Spectre.Console;

namespace aoc.Day08;

internal partial class Day08
{
	internal partial class Part1
	{
		private readonly Example example = new (
			"""
			RL

			AAA = (BBB, CCC)
			BBB = (DDD, EEE)
			CCC = (ZZZ, GGG)
			DDD = (DDD, DDD)
			EEE = (EEE, EEE)
			GGG = (GGG, GGG)
			ZZZ = (ZZZ, ZZZ)
			""", 2
		);

		private readonly Example repeatInstructions = new(
			"""
			LLR

			AAA = (BBB, BBB)
			BBB = (AAA, ZZZ)
			ZZZ = (ZZZ, ZZZ)
			""", 6
		);
		
		public Tuple<day08.Direction[], FSharpMap<string, Tuple<string, string>>> Parse(string input) 
			=> day08.parse(input);

		public int Solve(Tuple<day08.Direction[], FSharpMap<string, Tuple<string, string>>> input)
			=> day08.solve1(input.Item1, input.Item2);
	}

	internal partial class Part2
	{
		private readonly Example example = new(
			"""
			LR

			11A = (11B, XXX)
			11B = (XXX, 11Z)
			11Z = (11B, XXX)
			22A = (22B, XXX)
			22B = (22C, 22C)
			22C = (22Z, 22Z)
			22Z = (22B, 22B)
			XXX = (XXX, XXX)
			""", 6
		);
		
		public Tuple<day08.Direction[], FSharpMap<string, Tuple<string, string>>> Parse(string input) 
			=> day08.parse(input);

		public long Solve(Tuple<day08.Direction[], FSharpMap<string, Tuple<string, string>>> input)
			=> day08.solve2(input.Item1, input.Item2);
	}
}