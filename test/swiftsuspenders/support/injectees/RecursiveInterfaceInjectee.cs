using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class RecursiveInterfaceInjectee : Interface
	{
		[Inject]
		public InterfaceInjectee interfaceInjectee;
	}
}

