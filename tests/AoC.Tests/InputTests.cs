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
		yield return new PartInputCaseData(10, 1, "7063");
		yield return new PartInputCaseData(10, 2, "589");
		yield return new PartInputCaseData(11, 1, "9686930");
		yield return new PartInputCaseData(11, 2, "630728425490");
		yield return new PartInputCaseData(12, 1, "6949");
		yield return new PartInputCaseData(12, 2, "51456609952403");
		yield return new PartInputCaseData(13, 1, "34100");
		yield return new PartInputCaseData(13, 2, "33106");
		yield return new PartInputCaseData(14, 1, "105461");
		yield return new PartInputCaseData(14, 2, "102829");
		yield return new PartInputCaseData(15, 1, "508552");
		yield return new PartInputCaseData(15, 2, "265462");
		yield return new PartInputCaseData(16, 1, "7415");
		yield return new PartInputCaseData(16, 2, "7943");
		yield return new PartInputCaseData(17, 1, "771");
		yield return new PartInputCaseData(17, 2, "930");
		yield return new PartInputCaseData(18, 1, "28911");
		yield return new PartInputCaseData(18, 2, "77366737561114");
		yield return new PartInputCaseData(19, 1, "406849");
		yield return new PartInputCaseData(19, 2, "138625360533574");
		yield return new PartInputCaseData(20, 1, "818649769");
		yield return new PartInputCaseData(20, 2, "246313604784977");
		yield return new PartInputCaseData(21, 1, "3830");
		yield return new PartInputCaseData(21, 2, "637087163925555");
		yield return new PartInputCaseData(23, 1, "2318");
		yield return new PartInputCaseData(23, 2, "6426");
	}
}