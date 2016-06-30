using System;
using SwiftSuspenders.Support.Types;

namespace SwiftSuspenders.Support.Injectees
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

