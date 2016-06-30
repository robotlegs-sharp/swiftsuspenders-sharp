using System;
using SwiftSuspenders.Support.Types;
using SwiftSuspenders.Support.Enums;

namespace SwiftSuspenders.Support.Injectees
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

