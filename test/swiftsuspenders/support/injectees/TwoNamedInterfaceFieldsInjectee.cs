using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class TwoNamedInterfaceFieldsInjectee
	{
		[Inject("Name1")]
		public Interface property1;
		[Inject("Name2")]
		public Interface property2;
	}
}

