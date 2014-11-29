using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class OptionalClassInjectee
	{
		[Inject(true)]
		public Interface property;
	}
}

