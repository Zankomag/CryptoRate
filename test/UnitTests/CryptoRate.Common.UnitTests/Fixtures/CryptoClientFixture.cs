using System.Collections.Generic;
using System.Linq;

namespace CryptoRate.Common.UnitTests.Fixtures {

	public class CryptoClientFixture {

		public static IEnumerable<object[]> WrongApiKeysForJson =>
			new List<object[]> {
				new object[] {null},
				new object[] {""},
				new object[] {" "},
				new object[] {"    "},
				new object[] {" "}, //alt + 255
				new object[] {" MyRightApiKey"},
				new object[] {" MyRightApiKey "},
				new object[] {"MyRightApiKey "}
			};

		public static IEnumerable<object[]> WrongApiKeys => WrongApiKeysForJson.Concat(new List<object[]> {
			new object[] {"\t"},
			new object[] {"\n"}
		});

		public static IEnumerable<object[]> RightApiKeys =>
			new List<object[]> {
				new object[] {"MyRightApiKey"}
			};

	}

}