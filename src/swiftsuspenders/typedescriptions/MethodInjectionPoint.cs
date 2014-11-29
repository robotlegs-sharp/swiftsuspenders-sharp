using System;
using System.Reflection;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.errors;
using System.Collections.Generic;

namespace swiftsuspenders.typedescriptions
{
	public class MethodInjectionPoint : MethodBaseInjectionPoint
	{
		public MethodInjectionPoint (MethodInfo methodInfo, object[] keys, bool optional) : base(methodInfo, keys, optional)
		{

		}
	}
}

