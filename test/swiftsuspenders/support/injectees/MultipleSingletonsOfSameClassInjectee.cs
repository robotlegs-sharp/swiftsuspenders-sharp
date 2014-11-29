using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class MultipleSingletonsOfSameClassInjectee
	{
		[Inject]
		public Interface property1 { get; set; }

		[Inject]
		public Interface2 property2 { get; set; }
	}
}

