using System;
using System.Collections.Generic;

namespace swiftsuspenders.typedescriptions
{
	public class InjectionPoint
	{
		public InjectionPoint next;
		public InjectionPoint last;
		public Dictionary<string, object> injectParameters;

		public virtual void ApplyInjection(object target, Type targetType, Injector injector)
		{

		}
	}
}

