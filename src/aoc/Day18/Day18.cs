namespace aoc.Day18;

internal partial class Day18
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			R 6 (#70c710)
			D 5 (#0dc571)
			L 2 (#5713f0)
			D 2 (#d2c081)
			R 2 (#59c680)
			D 2 (#411b91)
			L 5 (#8ceee2)
			U 2 (#caa173)
			L 1 (#1b58a2)
			U 2 (#caa171)
			R 2 (#7807d2)
			U 3 (#a77fa3)
			L 2 (#015232)
			U 2 (#7a21e3)
			""", 62);

		public common.Point<long>[] Parse(string input) => day18.Part1.parse(input);
		public int Solve(common.Point<long>[] input) => (int)day18.solve(input);
	}

	internal partial class Part2
	{
		private readonly Example example = new(
			"""
			R 6 (#70c710)
			D 5 (#0dc571)
			L 2 (#5713f0)
			D 2 (#d2c081)
			R 2 (#59c680)
			D 2 (#411b91)
			L 5 (#8ceee2)
			U 2 (#caa173)
			L 1 (#1b58a2)
			U 2 (#caa171)
			R 2 (#7807d2)
			U 3 (#a77fa3)
			L 2 (#015232)
			U 2 (#7a21e3)
			""", 952408144115);

		public common.Point<long>[] Parse(string input) => day18.Part2.parse(input);
		public long Solve(common.Point<long>[] input) => day18.solve(input);
	}
}