using System;
using System.Reflection;

namespace SwiftSuspenders.TypeDescriptions
{
	public class PostConstructInjectionPoint : OrderedInjectionPoint
	{
		public PostConstructInjectionPoint (int order, MethodInfo methodInfo) : base(order, methodInfo)
		{

		}
	}
}

