using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class NamedClassInjectee
	{
		public const string NAME = "Name";

		[Inject(NAME)]
		public Clazz property { get; set; }
	}
}

