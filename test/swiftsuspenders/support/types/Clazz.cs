using System;

namespace swiftsuspenders.support.types
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

