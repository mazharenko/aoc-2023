namespace aoc.Day10;

internal partial class Day10
{
	internal partial class Part1
	{
		private readonly Example example1 = new(
			"""
			.....
			.S-7.
			.|.|.
			.L-J.
			.....
			""", 4);

		private readonly Example example2 = new(
			"""
			7-F7-
			.FJ|7
			SJLL7
			|F--J
			LJ.LJ
			""", 8);

		public int Solve(day10.Pipe[,] input) => day10.solve1(input);

		public day10.Pipe[,] Parse(string input) => day10.parse(input);
	}

	internal partial class Part2
	{
		private readonly Example example1 = new(
			"""
			...........
			.S-------7.
			.|F-----7|.
			.||.....||.
			.||.....||.
			.|L-7.F-J|.
			.|..|.|..|.
			.L--J.L--J.
			...........
			""", 4);
		
		private readonly Example example2 = new(
			"""
			..........
			.S------7.
			.|F----7|.
			.||....||.
			.||....||.
			.|L-7F-J|.
			.|..||..|.
			.L--JL--J.
			..........
			""", 4);

		private readonly Example example3 = new(
			"""
			FF7FSF7F7F7F7F7F---7
			L|LJ||||||||||||F--J
			FL-7LJLJ||||||LJL-77
			F--JF--7||LJLJ7F7FJ-
			L---JF-JLJ.||-FJLJJ7
			|F|F-JF---7F7-L7L|7|
			|FFJF7L7F-JF7|JL---7
			7-L-JL7||F7|L7F-7F7|
			L.L7LFJ|||||FJL7||LJ
			L7JLJL-JLJLJL--JLJ.L
			""", 10);

		public int Solve(day10.Pipe[,] input) => day10.solve2(input);
		public day10.Pipe[,] Parse(string input) => day10.parse(input);
	}
}