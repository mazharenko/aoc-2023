using mazharenko.AoCAgent.Generator;

namespace aoc.Day21;

internal partial class Day21
{
	[BypassNoExamples]
	internal partial class Part1
	{
		public char[,] Parse(string input) => day21.parse(input);
		public int Solve(char[,] input) => day21.solve1(input);
	}

	[BypassNoExamples]
	internal partial class Part2
	{
		public char[,] Parse(string input) => day21.parse(input);
		public long Solve(char[,] input) => day21.solve2(input);
	}
}