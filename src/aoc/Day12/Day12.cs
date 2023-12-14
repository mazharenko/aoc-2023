using mazharenko.AoCAgent.Generator;

namespace aoc.Day12;

internal partial class Day12
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			???.### 1,1,3
			.??..??...?##. 1,1,3
			?#?#?#?#?#?#?#? 1,3,1,6
			????.#...#... 4,1,1
			????.######..#####. 1,6,5
			?###???????? 3,2,1
			""", 21);

		private readonly Example example1 = new(".??#?#.??.?????????? 5,3", 8);

		public Tuple<string, int[]>[] Parse(string input) => day12.parse(input);
		public long Solve(Tuple<string, int[]>[] input) => day12.solve1(input);
	}

	internal partial class Part2
	{
		private readonly Example example1 = new("???.### 1,1,3", 1);
		private readonly Example example2 = new(".??..??...?##. 1,1,3", 16384);
		private readonly Example example3 = new("?#?#?#?#?#?#?#? 1,3,1,6", 1);
		private readonly Example example5 = new("????.#...#... 4,1,1", 16);
		private readonly Example example6 = new("????.######..#####. 1,6,5", 2500);
		private readonly Example example7 = new("?###???????? 3,2,1", 506250);
		
		public Tuple<string, int[]>[] Parse(string input) => day12.parse(input);
		public long Solve(Tuple<string, int[]>[] input) => day12.solve2(input);
	}
}