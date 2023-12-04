namespace aoc.Day01;

public partial class Day01
{
	public partial class Part1
	{
		private readonly Example example = new(
			"""
			1abc2
			pqr3stu8vwx
			a1b2c3d4e5f
			treb7uchet
			""",
			142
		);

		public string[] Parse(string input) => day01.parse(input);
		public int Solve(string[] input) => day01.solve1(input);
	}

	public partial class Part2
	{
		private readonly Example example = new(
			"""
			two1nine
			eightwothree
			abcone2threexyz
			xtwone3four
			4nineeightseven2
			zoneight234
			7pqrstsixteen
			""", 281
		);
		private readonly Example example2 = new("twoneighthree", 23);
		
		public string[] Parse(string input) => day01.parse(input);
		public int Solve(string[] input) => day01.solve2(input);
	}
}