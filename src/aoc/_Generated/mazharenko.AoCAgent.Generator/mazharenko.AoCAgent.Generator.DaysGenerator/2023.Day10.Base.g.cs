﻿// <auto-generated />
#nullable restore
using mazharenko.AoCAgent.Generator;
using mazharenko.AoCAgent;
using mazharenko.AoCAgent.Base;

namespace aoc.Day10
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.AoCAgent.Generator", "4.2.0")]
	partial class Day10
	{
		private interface IPart1 : IPart
		{
			int Solve(global::impl.day10.Pipe[, ] input);
			global::impl.day10.Pipe[, ] Parse(string input);
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
				yield return new NamedExample("example1", new ExampleAdapter<int>(example1));
				yield return new NamedExample("example2", new ExampleAdapter<int>(example2));
			}

			public string SolveString(string input)
			{
				var parsedInput = Parse(input);
				return Format(Solve(parsedInput));
			}

			private record Example(string Input, int Expectation) : IExample<int>
			{
				private string expectationFormatted = null !;
				private static global::aoc.Day10.Day10.Part1 part = new aoc.Day10.Day10.Part1();
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
			int Solve(global::impl.day10.Pipe[, ] input);
			global::impl.day10.Pipe[, ] Parse(string input);
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
				yield return new NamedExample("example1", new ExampleAdapter<int>(example1));
				yield return new NamedExample("example2", new ExampleAdapter<int>(example2));
				yield return new NamedExample("example3", new ExampleAdapter<int>(example3));
			}

			public string SolveString(string input)
			{
				var parsedInput = Parse(input);
				return Format(Solve(parsedInput));
			}

			private record Example(string Input, int Expectation) : IExample<int>
			{
				private string expectationFormatted = null !;
				private static global::aoc.Day10.Day10.Part2 part = new aoc.Day10.Day10.Part2();
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