﻿// <auto-generated />
#nullable restore
using mazharenko.AoCAgent.Generator;
using mazharenko.AoCAgent;
using mazharenko.AoCAgent.Base;

namespace aoc.Day25
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.AoCAgent.Generator", "5.0.0")]
	partial class Day25
	{
		private interface IPart1 : IPart
		{
			int Solve(global::impl.day25.G input);
			global::impl.day25.G Parse(string input);
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
				private static global::aoc.Day25.Day25.Part1 part = new aoc.Day25.Day25.Part1();
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
			string Solve(string input);
			string Parse(string input);
		}

		abstract internal class Part2Base
		{
			public virtual string Format(string res) => res!.ToString()!;
			public virtual string Parse(string input) => input.Trim();
		}

		partial class Part2 : Part2Base, IPart2
		{
			public Settings Settings { get; } = new Settings
			{
				BypassNoExamples = false,
				ManualInterpretation = false
			};

			public override string Format(string res) => res;
			public System.Collections.Generic.IEnumerable<NamedExample> GetExamples()
			{
				yield break;
			}

			public string SolveString(string input)
			{
				var parsedInput = Parse(input);
				return Format(Solve(parsedInput));
			}

			private record Example(string Input, string Expectation) : IExample<string>
			{
				private string expectationFormatted = null !;
				private static global::aoc.Day25.Day25.Part2 part = new aoc.Day25.Day25.Part2();
				public string Run()
				{
					var parsedInput = part.Parse(Input);
					return part.Solve(parsedInput);
				}

				public string RunFormat(out string formatted)
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