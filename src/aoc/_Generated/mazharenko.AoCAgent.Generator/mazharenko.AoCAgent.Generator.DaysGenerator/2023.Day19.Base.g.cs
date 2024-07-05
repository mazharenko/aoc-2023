﻿// <auto-generated />
#nullable restore
using mazharenko.AoCAgent.Generator;
using mazharenko.AoCAgent;
using mazharenko.AoCAgent.Base;

namespace aoc.Day19
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.AoCAgent.Generator", "5.0.0")]
	partial class Day19
	{
		private interface IPart1 : IPart
		{
			long Solve(global::System.Tuple<global::Microsoft.FSharp.Collections.FSharpMap<string, global::impl.day19.Wf>, global::Microsoft.FSharp.Collections.FSharpMap<string, long>[]> input);
			global::System.Tuple<global::Microsoft.FSharp.Collections.FSharpMap<string, global::impl.day19.Wf>, global::Microsoft.FSharp.Collections.FSharpMap<string, long>[]> Parse(string input);
		}

		abstract internal class Part1Base
		{
			public virtual string Format(long res) => res!.ToString()!;
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
				yield return new NamedExample("example", new ExampleAdapter<long>(example));
			}

			public string SolveString(string input)
			{
				var parsedInput = Parse(input);
				return Format(Solve(parsedInput));
			}

			private record Example(string Input, long Expectation) : IExample<long>
			{
				private string expectationFormatted = null !;
				private static global::aoc.Day19.Day19.Part1 part = new aoc.Day19.Day19.Part1();
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

		private interface IPart2 : IPart
		{
			long Solve(global::Microsoft.FSharp.Collections.FSharpMap<string, global::impl.day19.Wf> input);
			global::Microsoft.FSharp.Collections.FSharpMap<string, global::impl.day19.Wf> Parse(string input);
		}

		abstract internal class Part2Base
		{
			public virtual string Format(long res) => res!.ToString()!;
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
				yield return new NamedExample("example", new ExampleAdapter<long>(example));
			}

			public string SolveString(string input)
			{
				var parsedInput = Parse(input);
				return Format(Solve(parsedInput));
			}

			private record Example(string Input, long Expectation) : IExample<long>
			{
				private string expectationFormatted = null !;
				private static global::aoc.Day19.Day19.Part2 part = new aoc.Day19.Day19.Part2();
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