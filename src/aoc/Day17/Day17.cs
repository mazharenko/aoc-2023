using mazharenko.AoCAgent.Generator;

namespace aoc.Day17;

internal partial class Day17
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			2413432311323
			3215453535623
			3255245654254
			3446585845452
			4546657867536
			1438598798454
			4457876987766
			3637877979653
			4654967986887
			4564679986453
			1224686865563
			2546548887735
			4322674655533
			""", 102);

		public int[,] Parse(string input) => day17.parse(input);
		public int Solve(int[,] input) => day17.solve1(input);
	}

	internal partial class Part2
	{
		private readonly Example example1 = new(
			"""
			2413432311323
			3215453535623
			3255245654254
			3446585845452
			4546657867536
			1438598798454
			4457876987766
			3637877979653
			4654967986887
			4564679986453
			1224686865563
			2546548887735
			4322674655533
			""", 94);
		
		private readonly Example example2 = new(
			"""
			111111111111
			999999999991
			999999999991
			999999999991
			999999999991
			""", 71);
		

		public int[,] Parse(string input) => day17.parse(input);
		public int Solve(int[,] input) => day17.solve2(input);

	}
}