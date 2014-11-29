using System;
using swiftsuspenders.support.types;
using swiftsuspenders.support.enums;

namespace swiftsuspenders.support.injectees
{
	public class NamedInterfaceInjectee
	{
		[Inject(InjectEnum.NAME)]
		public Interface property { get; set; }

		public NamedInterfaceInjectee()
		{
		}
	}
}

