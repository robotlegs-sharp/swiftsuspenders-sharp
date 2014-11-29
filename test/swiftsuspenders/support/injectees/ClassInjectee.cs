using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
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

