using Microsoft.FSharp.Collections;

namespace aoc.Day06;

internal partial class Day06
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			Time:      7  15   30
			Distance:  9  40  200
			""", 288);

		public FSharpList<Tuple<long, long>> Parse(string input) => day06.parse(input);
		public int Solve(FSharpList<Tuple<long, long>> input) => day06.solve(input);
	}

	internal partial class Part2
	{
		private readonly Example example = new(
			"""
			Time:      7  15   30
			Distance:  9  40  200
			""", 71503);

		public FSharpList<Tuple<long, long>> Parse(string input)
		{
			return day06.parse(input.Replace(" ", ""));
		}

		public long Solve(FSharpList<Tuple<long, long>> input)
		{
			return day06.solve(input);
		}
	}
}