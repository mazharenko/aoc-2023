namespace aoc.Day16;

internal partial class Day16
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			.|...\....
			|.-.\.....
			.....|-...
			........|.
			..........
			.........\
			..../.\\..
			.-.-/..|..
			.|....-|.\
			..//.|....
			""", 46);

		public char[,] Parse(string input) => day16.parse(input);
		public int Solve(char[,] input) => day16.solve1(input);
	}

	internal partial class Part2
	{
		private readonly Example example = new(
			"""
			.|...\....
			|.-.\.....
			.....|-...
			........|.
			..........
			.........\
			..../.\\..
			.-.-/..|..
			.|....-|.\
			..//.|....
			""", 51);

		public char[,] Parse(string input) => day16.parse(input);
		public int Solve(char[,] input) => day16.solve2(input);
	}
}