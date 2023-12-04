namespace aoc.Day03;

internal partial class Day03
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			467..114..
			...*......
			..35..633.
			......#...
			617*......
			.....+.58.
			..592.....
			......755.
			...$.*....
			.664.598..
			""", 4361);

		private readonly Example lineEndsWithNumber = new(
			"""
			..35...633
			.......*..
			"""
			, 633);
		
		public char[,] Parse(string input) => day03.parse(input);
		public int Solve(char[,] input) => day03.solve1(input);
	}

	internal partial class Part2
	{
		private readonly Example example = new(
			"""
			467..114..
			...*......
			..35..633.
			......#...
			617*......
			.....+.58.
			..592.....
			......755.
			...$.*....
			.664.598..
			""", 467835);

		private readonly Example lineEndsWithNumber = new(
			"""
			..35...633
			.....12*..
			"""
			, 7596);
		
		public char[,] Parse(string input) => day03.parse(input);
		public int Solve(char[,] input) => day03.solve2(input);
	}
}