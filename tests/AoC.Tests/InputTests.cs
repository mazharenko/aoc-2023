using System.Collections.Generic;
using mazharenko.AoCAgent.Generator;

namespace AoC.Tests;

[GenerateInputTests(nameof(GetCases))]
[TestFixture]
public partial class InputTests
{
	private static IEnumerable<PartInputCaseData> GetCases()
	{
		yield return new PartInputCaseData(1,1, "56049");
		yield return new PartInputCaseData(1,2, "54530");
		yield return new PartInputCaseData(2, 1, "2679");
		yield return new PartInputCaseData(2, 2, "77607");
		yield return new PartInputCaseData(3, 1, "533784");
		yield return new PartInputCaseData(3, 2, "78826761");
		yield return new PartInputCaseData(4, 1, "25174");
		yield return new PartInputCaseData(4, 2, "6420979");
		yield return new PartInputCaseData(5, 1, "322500873");
		yield return new PartInputCaseData(5, 2, "108956227");
		yield return new PartInputCaseData(6, 1, "771628");
		yield return new PartInputCaseData(6, 2, "27363861");
		yield return new PartInputCaseData(7, 1, "250951660");
		yield return new PartInputCaseData(7, 2, "251481660");
		yield return new PartInputCaseData(8, 1, "16897");
		yield return new PartInputCaseData(8, 2, "16563603485021");
		yield return new PartInputCaseData(9, 1, "1681758908");
		yield return new PartInputCaseData(9, 2, "803");
	}
}