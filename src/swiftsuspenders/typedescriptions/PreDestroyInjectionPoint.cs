using System;
using System.Reflection;

namespace swiftsuspenders.typedescriptions
{
	public class PreDestroyInjectionPoint : OrderedInjectionPoint
	{
		public PreDestroyInjectionPoint (int order, MethodInfo methodInfo) : base(order, methodInfo)
		{

		}
	}
}

