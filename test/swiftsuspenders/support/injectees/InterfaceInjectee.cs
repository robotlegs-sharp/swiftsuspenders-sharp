using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
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

