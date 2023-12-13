using mazharenko.AoCAgent.Generator;

namespace aoc.Day13;

internal partial class Day13
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			#.##..##.
			..#.##.#.
			##......#
			##......#
			..#.##.#.
			..##..##.
			#.#.##.#.

			#...##..#
			#....#..#
			..##..###
			#####.##.
			#####.##.
			..##..###
			#....#..#
			""", 405);

		private readonly Example narrowBottom = new(
			"""
			#.##.#..#.#
			..##.#.#.##
			########...
			########...
			""", 300);
		
		private readonly Example narrowTop = new(
			"""
			########...
			########...
			#.##.#..#.#
			..##.#.#.##
			""", 100);

		private readonly Example narrowRight = new(
			"""
			##.##
			.#.##
			..###
			.#.##
			""", 4);
		
		private readonly Example narrowLeft = new(
			"""
			####.
			##.#.
			##..#
			##.#.
			""", 1);
		
		public char[][,] Parse(string input) => day13.parse(input);
		public int Solve(char[][,] input) => day13.solve1(input);
	}

	internal partial class Part2
	{
		private readonly Example example = new(
			"""
			#.##..##.
			..#.##.#.
			##......#
			##......#
			..#.##.#.
			..##..##.
			#.#.##.#.
			
			#...##..#
			#....#..#
			..##..###
			#####.##.
			#####.##.
			..##..###
			#....#..#
			""", 400);
		public char[][,] Parse(string input) => day13.parse(input);
		public int Solve(char[][,] input) => day13.solve2(input);
	}
}