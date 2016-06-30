using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class MultipleSingletonsOfSameClassInjectee
	{
		[Inject]
		public Interface property1 { get; set; }

		[Inject]
		public Interface2 property2 { get; set; }
	}
}

