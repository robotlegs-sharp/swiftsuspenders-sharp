using System;
using System.Collections.Generic;

namespace SwiftSuspenders.TypeDescriptions
{
	public class InjectionPoint
	{
		public InjectionPoint next;
		public InjectionPoint last;
		public Dictionary<string, object> injectParameters; //TODO: Remove this variable and check we don't need it

		public virtual void ApplyInjection(object target, Type targetType, Injector injector)
		{

		}
	}
}

