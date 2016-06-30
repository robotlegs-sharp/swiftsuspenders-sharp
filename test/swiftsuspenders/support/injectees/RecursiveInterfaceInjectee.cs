using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class RecursiveInterfaceInjectee : Interface
	{
		[Inject]
		public InterfaceInjectee interfaceInjectee;
	}
}

