﻿// <auto-generated />
#nullable restore
using mazharenko.AoCAgent.Generator;
using mazharenko.AoCAgent;
using mazharenko.AoCAgent.Base;

namespace aoc.Day04
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.AoCAgent.Generator", "4.2.0")]
	partial class Day04
	{
		private interface IPart1 : IPart
		{
			int Solve(global::impl.day04.Card[] input);
			global::impl.day04.Card[] Parse(string input);
		}

		abstract internal class Part1Base
		{
			public virtual string Format(int res) => res!.ToString()!;
		}

		partial class Part1 : Part1Base, IPart1
		{
			public Settings Settings { get; } = new Settings
			{
				BypassNoExamples = false,
				ManualInterpretation = false
			};

			public System.Collections.Generic.IEnumerable<NamedExample> GetExamples()
			{
				yield return new NamedExample("example", new ExampleAdapter<int>(example));
			}

			public string SolveString(string input)
			{
				var parsedInput = Parse(input);
				return Format(Solve(parsedInput));
			}

			private record Example(string Input, int Expectation) : IExample<int>
			{
				private string expectationFormatted = null !;
				private static global::aoc.Day04.Day04.Part1 part = new aoc.Day04.Day04.Part1();
				public int Run()
				{
					var parsedInput = part.Parse(Input);
					return part.Solve(parsedInput);
				}

				public int RunFormat(out string formatted)
				{
					var res = Run();
					formatted = part.Format(res);
					return res;
				}

				public string ExpectationFormatted => expectationFormatted ??= part.Format(Expectation);
			}
		}

		private interface IPart2 : IPart
		{
			int Solve(global::impl.day04.Card[] input);
			global::impl.day04.Card[] Parse(string input);
		}

		abstract internal class Part2Base
		{
			public virtual string Format(int res) => res!.ToString()!;
		}

		partial class Part2 : Part2Base, IPart2
		{
			public Settings Settings { get; } = new Settings
			{
				BypassNoExamples = false,
				ManualInterpretation = false
			};

			public System.Collections.Generic.IEnumerable<NamedExample> GetExamples()
			{
				yield return new NamedExample("example", new ExampleAdapter<int>(example));
			}

			public string SolveString(string input)
			{
				var parsedInput = Parse(input);
				return Format(Solve(parsedInput));
			}

			private record Example(string Input, int Expectation) : IExample<int>
			{
				private string expectationFormatted = null !;
				private static global::aoc.Day04.Day04.Part2 part = new aoc.Day04.Day04.Part2();
				public int Run()
				{
					var parsedInput = part.Parse(Input);
					return part.Solve(parsedInput);
				}

				public int RunFormat(out string formatted)
				{
					var res = Run();
					formatted = part.Format(res);
					return res;
				}

				public string ExpectationFormatted => expectationFormatted ??= part.Format(Expectation);
			}
		}
	}
}