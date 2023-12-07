namespace aoc.Day07;

internal partial class Day07
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			32T3K 765
			T55J5 684
			KK677 28
			KTJJT 220
			QQQJA 483
			""", 6440);

		public Tuple<day07.Part1.Hand, int>[] Parse(string input) => day07.Part1.parse(input);
		public int Solve(Tuple<day07.Part1.Hand, int>[] input) => day07.Part1.solve(input);
	}

	internal partial class Part2
	{
			private readonly Example example = new(
			"""
			32T3K 765
			T55J5 684
			KK677 28
			KTJJT 220
			QQQJA 483
			""", 5905);

		public Tuple<day07.Part2.Hand, int>[] Parse(string input) => day07.Part2.parse(input);
		public int Solve(Tuple<day07.Part2.Hand, int>[] input) => day07.Part2.solve(input);
	}
}