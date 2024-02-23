using mazharenko.AoCAgent.Generator;
using Microsoft.Z3;

namespace aoc.Day24;

internal partial class Day24
{
	[BypassNoExamples]
	internal partial class Part1
	{
		// this day is solved with Z3 SMT-solver in C# not in F#
		// because F# fails to understand overloaded comparison
		// operators e.g. ArithExpr.>=
		
		public day24.Ray[] Parse(string input) => day24.parse(input);

		public long Solve(day24.Ray[] input)
		{
			// do our best to reuse the context and the state of the solver
			// in order to achieve decent performance
			using var ctx = new Context();
			var solver = ctx.MkSimpleSolver();
			var tA = ctx.MkRealConst("t_a");
			var tB = ctx.MkRealConst("t_b");
			var intersectionX = ctx.MkRealConst("intersectionX");
			var intersectionY = ctx.MkRealConst("intersectionY");

			// to determine if two rays A and B intersect, we need to solve an equation system:
			// | x = A1x + (A2x - A1x)t_a
			// | y = A1y + (A2y - A1y)t_a 
			// | x = B1x + (B2x - B1x)t_b
			// | y = B1y + (B2y - B1y)t_b
			// The rays intersect if and only if t_a>=0 and t_b>=0
			// Along with this condition here go additional conditions for the test area
			
			solver.Add(tA >= 0 & tB >= 0);
			solver.Add(intersectionX >= 200000000000000 & intersectionX <= 400000000000000);
			solver.Add(intersectionY >= 200000000000000 & intersectionY <= 400000000000000);
			
			// These assertions remain the same for all ray pairs.
			
			var count = 0;
			
			// Then we want to create assertion objects for all the rays in advance to
			// save some allocations

			var inputEquations = input.Select(xx =>
			{
				var x = ctx.MkReal(xx.x);
				var dx = ctx.MkReal(xx.dx);
				var y = ctx.MkReal(xx.y);
				var dy = ctx.MkReal(xx.dy);
				return new
				{
					// almost each of the rays can be both A and B in the equation system above
					EqAsA = ctx.MkEq(intersectionX, x + dx * tA)
					        & ctx.MkEq(intersectionY, y + dy * tA),
					EqAsB = ctx.MkEq(intersectionX, x + dx * tB)
					        & ctx.MkEq(intersectionY, y + dy * tB)
				};
			}).ToList();
			
			// Iterate over ray pairs in a way that allows to reuse states
			// of the solver
			for (var iA = 0; iA < inputEquations.Count; iA++) // ray A
			{
				// There are already some assertions in the state of the solver.
				// To move on to the next A ray, we create a backtracking point
				// and add the corresponding assertion.
				solver.Push();
				
				solver.Add(inputEquations[iA].EqAsA);
				
				for (var iB = iA + 1; iB < inputEquations.Count; iB++) // ray B
				{
					// There are already some assertions in the state of the solver
					// including assertions for the A ray. To move on to the next B ray,
					// we create a backtracking point and add the corresponding assertion.
					solver.Push();
					
					solver.Add(inputEquations[iB].EqAsB);
					
					// If the system has a solution, it means that the rays intersect.
					if (solver.Check() == Status.SATISFIABLE)
						count++;
					
					// Rollback the solver to the previous state. Now there are only
					// common assertions and assertions for the A ray
					solver.Pop();
				}
				
				// Rollback the solver to the previous state. Now there are only common
				// assertions
				solver.Pop();
			}

			return count;
		}
	}

	[BypassNoExamples]
	internal partial class Part2
	{
		public string Solve(string input)
		{
			throw new NotImplementedException();
		}
	}
}