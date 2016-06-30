using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class OptionalClassInjectee
	{
		[Inject(true)]
		public Interface property;
	}
}

