﻿// <auto-generated />
#nullable restore
using Spectre.Console;
using System.Threading.Tasks;
using System;

[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.AoCAgent.Generator", "5.0.0")]
internal class Program
{
	private static async Task Main(string[] args)
	{
		try
		{
			await new mazharenko.AoCAgent.Runner().Run(new aoc.Year2023());
		}
		catch (Exception e)
		{
			AnsiConsole.WriteException(e);
			Environment.Exit(134);
		}
	}
}