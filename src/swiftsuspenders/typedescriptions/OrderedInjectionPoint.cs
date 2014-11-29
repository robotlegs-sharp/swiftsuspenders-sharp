using System;
using System.Reflection;

namespace swiftsuspenders.typedescriptions
{
	public class OrderedInjectionPoint : MethodBaseInjectionPoint
	{
		public int order;

		public OrderedInjectionPoint (int order, MethodInfo methodInfo) : base(methodInfo)
		{
			this.order = order;
		}
	}
}

