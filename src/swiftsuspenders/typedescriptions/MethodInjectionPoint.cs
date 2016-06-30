using System;
using System.Reflection;
using SwiftSuspenders.DependencyProviders;
using SwiftSuspenders.Errors;
using System.Collections.Generic;

namespace SwiftSuspenders.TypeDescriptions
{
	public class MethodInjectionPoint : MethodBaseInjectionPoint
	{
		public MethodInjectionPoint (MethodInfo methodInfo, object[] keys, bool optional) : base(methodInfo, keys, optional)
		{

		}
	}
}

