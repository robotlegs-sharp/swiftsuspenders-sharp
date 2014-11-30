using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class PostConstructWithArgInjectee
	{
		public Clazz property;

		[PostConstruct]
		public void DoSomeStuff(Clazz arg)
		{
			property = arg;
		}
	}
}

