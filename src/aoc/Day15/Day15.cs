namespace aoc.Day15;

internal partial class Day15
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7
			""", 1320);

		public string[] Parse(string input) => day15.Part1.parse(input);
		public int Solve(string[] input) => day15.Part1.solve(input);
	}

	internal partial class Part2
	{
		private readonly Example example = new(
			"""
			rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7
			""", 145);

		public day15.Part2.Operation[] Parse(string input) => day15.Part2.parse(input);
		public int Solve(day15.Part2.Operation[] input) => day15.Part2.solve(input);
	}
}