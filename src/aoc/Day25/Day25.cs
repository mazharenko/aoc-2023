namespace aoc.Day25;

internal partial class Day25
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			jqt: rhn xhk nvd
			rsh: frs pzl lsr
			xhk: hfx
			cmg: qnr nvd lhk bvb
			rhn: xhk bvb hfx
			bvb: xhk hfx
			pzl: lsr hfx nvd
			qnr: nvd
			ntq: jqt hfx bvb xhk
			nvd: lhk
			lsr: lhk
			rzs: qnr cmg lsr rsh
			frs: qnr lhk lsr
			""", 54);

		public day25.G Parse(string input) => day25.parse(input);
		public int Solve(day25.G input) => day25.solve1(input);
	}

	internal partial class Part2
	{
		public string Solve(string input)
		{
			throw new NotImplementedException();
		}
	}
}