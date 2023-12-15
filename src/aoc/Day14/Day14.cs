namespace aoc.Day14;

internal partial class Day14
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			O....#....
			O.OO#....#
			.....##...
			OO.#O....O
			.O.....O#.
			O.#..O.#.#
			..O..#O..O
			.......O..
			#....###..
			#OO..#....
			""", 136);

		public char[,] Parse(string input) => day14.parse(input);
		public int Solve(char[,] input) => day14.solve1(input);
	}

	internal partial class Part2
	{
		private readonly Example example = new(
			"""
			O....#....
			O.OO#....#
			.....##...
			OO.#O....O
			.O.....O#.
			O.#..O.#.#
			..O..#O..O
			.......O..
			#....###..
			#OO..#....
			""", 64);

		public char[,] Parse(string input) => day14.parse(input);
		public int Solve(char[,] input) => day14.solve2(input);
	}
}