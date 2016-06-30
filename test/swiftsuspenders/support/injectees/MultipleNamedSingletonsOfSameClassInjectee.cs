using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class MultipleNamedSingletonsOfSameClassInjectee
	{
		[Inject]
		public Interface property1 { get; set; }

		[Inject]
		public Interface2 property2 { get; set; }

		[Inject("name1")]
		public Interface namedProperty1 { get; set; }

		[Inject("name2")]
		public Interface2 namedProperty2 { get; set; }
	}
}

