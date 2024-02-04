namespace aoc.Day22;

internal partial class Day22
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			1,0,1~1,2,1
			0,0,2~2,0,2
			0,2,3~2,2,3
			0,0,4~0,2,4
			2,0,5~2,2,5
			0,1,6~2,1,6
			1,1,8~1,1,9
			""", 5);

		public day22.Brick[] Parse(string input) => day22.parse(input);
		public int Solve(day22.Brick[] input) => day22.solve1(input);
	}

	internal partial class Part2
	{
		private readonly Example example = new(
			"""
			1,0,1~1,2,1
			0,0,2~2,0,2
			0,2,3~2,2,3
			0,0,4~0,2,4
			2,0,5~2,2,5
			0,1,6~2,1,6
			1,1,8~1,1,9
			""", 7);
		
		public day22.Brick[] Parse(string input) => day22.parse(input);
		public int Solve(day22.Brick[] input) => day22.solve2(input);
	}
}