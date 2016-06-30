using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
{
	public class ClassInjectee
	{
		[Inject]
		public Clazz property { get; set; }

		public bool someProperty;

		public ClassInjectee()
		{
			someProperty = false;
		}


		[PostConstruct(1)]
		public void DoSomeStuff()
		{
			someProperty = true;
		}
	}
}

