using System;
using System.Reflection;

namespace swiftsuspenders.typedescriptions
{
	public class PostConstructInjectionPoint : OrderedInjectionPoint
	{
		public PostConstructInjectionPoint (int order, MethodInfo methodInfo) : base(order, methodInfo)
		{

		}
	}
}

