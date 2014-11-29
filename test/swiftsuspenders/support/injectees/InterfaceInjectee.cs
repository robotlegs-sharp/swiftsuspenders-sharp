using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class InterfaceInjectee
	{
		[Inject]
		public Interface property { get; set; }

		public InterfaceInjectee()
		{
		}
	}
}

