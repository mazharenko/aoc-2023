using mazharenko.AoCAgent.Generator;
using Microsoft.FSharp.Collections;

namespace aoc.Day20;

internal partial class Day20
{
	internal partial class Part1
	{
		private readonly Example example1 = new(
			"""
			broadcaster -> a, b, c
			%a -> b
			%b -> c
			%c -> inv
			&inv -> a
			""", 32000000);

		private readonly Example example2 = new(
			"""
			broadcaster -> a
			%a -> inv, con
			&inv -> b
			%b -> con
			&con -> output
			""", 11687500);

		public FSharpMap<string, day20.Module> Parse(string input) => day20.parse(input);
		public long Solve(FSharpMap<string, day20.Module> input) => day20.solve1(input);
	}

	[BypassNoExamples]
	internal partial class Part2
	{
		public FSharpMap<string, day20.Module> Parse(string input) => day20.parse(input);
		public long Solve(FSharpMap<string, day20.Module> input) => day20.solve2(input);
	}
}