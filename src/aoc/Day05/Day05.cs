using Microsoft.FSharp.Collections;

namespace aoc.Day05;

internal partial class Day05
{
	internal partial class Part1
	{
		private readonly Example example = new(
			"""
			seeds: 79 14 55 13
			
			seed-to-soil map:
			50 98 2
			52 50 48

			soil-to-fertilizer map:
			0 15 37
			37 52 2
			39 0 15

			fertilizer-to-water map:
			49 53 8
			0 11 42
			42 0 7
			57 7 4

			water-to-light map:
			88 18 7
			18 25 70

			light-to-temperature map:
			45 77 23
			81 45 19
			68 64 13

			temperature-to-humidity map:
			0 69 1
			1 0 69

			humidity-to-location map:
			60 56 37
			56 93 4
			""", 35);

		public Tuple<FSharpList<long>, FSharpList<FSharpList<day05.RangeMap>>> Parse(string input) 
			=> day05.parse(input);

		public long Solve(Tuple<FSharpList<long>, FSharpList<FSharpList<day05.RangeMap>>> input) 
			=> day05.Part1.solve(input.Item1, input.Item2);

	}

	internal partial class Part2
	{
		private readonly Example example = new(
			"""
			seeds: 79 14 55 13

			seed-to-soil map:
			50 98 2
			52 50 48

			soil-to-fertilizer map:
			0 15 37
			37 52 2
			39 0 15

			fertilizer-to-water map:
			49 53 8
			0 11 42
			42 0 7
			57 7 4

			water-to-light map:
			88 18 7
			18 25 70

			light-to-temperature map:
			45 77 23
			81 45 19
			68 64 13

			temperature-to-humidity map:
			0 69 1
			1 0 69

			humidity-to-location map:
			60 56 37
			56 93 4
			""", 46);

		public Tuple<FSharpList<long>, FSharpList<FSharpList<day05.RangeMap>>> Parse(string input) 
			=> day05.parse(input);

		public long Solve(Tuple<FSharpList<long>, FSharpList<FSharpList<day05.RangeMap>>> input) 
			=> day05.Part2.solve(input.Item1, input.Item2);


	}
}