using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class NamedClassInjectee
	{
		public const string NAME = "Name";

		[Inject(NAME)]
		public Clazz property { get; set; }
	}
}

