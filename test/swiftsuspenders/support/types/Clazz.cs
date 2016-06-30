using System;

namespace SwiftSuspenders.Support.Types
{
	public class Clazz: Interface, Interface2
	{
		public bool preDestroyCalled;

		public Clazz ()
		{
		}

		[PreDestroy]
		public void preDestroy()
		{
			preDestroyCalled = true;
		}
	}
}

