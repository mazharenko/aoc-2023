﻿// <auto-generated />
#nullable restore
using mazharenko.AoCAgent.Generator;
using mazharenko.AoCAgent;
using mazharenko.AoCAgent.Base;

namespace aoc.Day21
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.AoCAgent.Generator", "5.0.0")]
	partial class Day21
	{
		private interface IPart1 : IPart
		{
			int Solve(char[, ] input);
			char[, ] Parse(string input);
		}

		abstract internal class Part1Base
		{
			public virtual string Format(int res) => res!.ToString()!;
		}

		partial class Part1 : Part1Base, IPart1
		{
			public Settings Settings { get; } = new Settings
			{
				BypassNoExamples = true,
				ManualInterpretation = false
			};

			public System.Collections.Generic.IEnumerable<NamedExample> GetExamples()
			{
				yield break;
			}

			public string SolveString(string input)
			{
				var parsedInput = Parse(input);
				return Format(Solve(parsedInput));
			}

			private record Example(string Input, int Expectation) : IExample<int>
			{
				private string expectationFormatted = null !;
				private static global::aoc.Day21.Day21.Part1 part = new aoc.Day21.Day21.Part1();
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
			long Solve(char[, ] input);
			char[, ] Parse(string input);
		}

		abstract internal class Part2Base
		{
			public virtual string Format(long res) => res!.ToString()!;
		}

		partial class Part2 : Part2Base, IPart2
		{
			public Settings Settings { get; } = new Settings
			{
				BypassNoExamples = true,
				ManualInterpretation = false
			};

			public System.Collections.Generic.IEnumerable<NamedExample> GetExamples()
			{
				yield break;
			}

			public string SolveString(string input)
			{
				var parsedInput = Parse(input);
				return Format(Solve(parsedInput));
			}

			private record Example(string Input, long Expectation) : IExample<long>
			{
				private string expectationFormatted = null !;
				private static global::aoc.Day21.Day21.Part2 part = new aoc.Day21.Day21.Part2();
				public long Run()
				{
					var parsedInput = part.Parse(Input);
					return part.Solve(parsedInput);
				}

				public long RunFormat(out string formatted)
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