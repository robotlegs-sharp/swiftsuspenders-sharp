using System;
using System.Reflection;

namespace SwiftSuspenders.TypeDescriptions
{
	public class PreDestroyInjectionPoint : OrderedInjectionPoint
	{
		public PreDestroyInjectionPoint (int order, MethodInfo methodInfo) : base(order, methodInfo)
		{

		}
	}
}

