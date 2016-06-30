using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class TwoNamedInterfaceFieldsInjectee
	{
		[Inject("Name1")]
		public Interface property1;
		[Inject("Name2")]
		public Interface property2;
	}
}

