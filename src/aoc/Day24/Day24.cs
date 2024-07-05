using mazharenko.AoCAgent.Generator;
using Microsoft.Z3;

namespace aoc.Day24;

internal partial class Day24
{
	// this day is solved with Z3 SMT-solver in C# not in F#
	// because F# fails to understand overloaded comparison
	// operators e.g. ArithExpr.>=
	
	[BypassNoExamples]
	internal partial class Part1
	{
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

			var inputEquations = input.Select(ray =>
				new
				{
					// almost each of the rays can be both A and B in the equation system above
					EqAsA = ctx.MkEq(intersectionX, ray.x + ray.dx * tA)
					        & ctx.MkEq(intersectionY, ray.y + ray.dy * tA),
					EqAsB = ctx.MkEq(intersectionX, ray.x + ray.dx * tB)
					        & ctx.MkEq(intersectionY, ray.y + ray.dy * tB)
				}
			).ToList();
			
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
		public day24.Ray[] Parse(string input) => day24.parse(input);

		public long Solve(day24.Ray[] input)
		{
			using var ctx = new Context();
			var solver = ctx.MkSolver();
			var x = ctx.MkRealConst("x")!;
			var y = ctx.MkRealConst("y")!;
			var z = ctx.MkRealConst("z")!;
			var dx = ctx.MkRealConst("dx")!;
			var dy = ctx.MkRealConst("dy")!;
			var dz = ctx.MkRealConst("dz")!;
			
			for (var i = 0; i < input.Length; i++)
			{
				var ray = input[i];
				var ti = ctx.MkRealConst($"t{i}")!;
				solver.Add(ti >= 0);
				solver.Add(ctx.MkEq(x + ti * dx, ray.x + ti * ray.dx));
				solver.Add(ctx.MkEq(y + ti * dy, ray.y + ti * ray.dy));
				solver.Add(ctx.MkEq(z + ti * dz, ray.z + ti * ray.dz));
			}

			solver.Check();
			
			return (long)((RatNum)solver.Model.Eval(x + y + z)).Double;
		}
	}
}