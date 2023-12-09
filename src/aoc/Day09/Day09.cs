namespace aoc.Day09;

internal partial class Day09
{
	internal partial class Part1
	{
		private readonly Example example1 = new("0 3 6 9 12 15", 18);
		private readonly Example example2 = new("1 3 6 10 15 21", 28);
		private readonly Example example3 = new("10 13 16 21 30 45", 68);

		private readonly Example example = new(
			"""
			0 3 6 9 12 15
			1 3 6 10 15 21
			10 13 16 21 30 45
			""", 114);

		public int Solve(int[][] input) => day09.solve1(input);
		public int[][] Parse(string input) => day09.parse(input);
	}

	internal partial class Part2
	{
		private readonly Example example = new(
			"""
			0 3 6 9 12 15
			1 3 6 10 15 21
			10 13 16 21 30 45
			""", 2);
		public int Solve(int[][] input) => day09.solve2(input);
		public int[][] Parse(string input) => day09.parse(input);
	}
}