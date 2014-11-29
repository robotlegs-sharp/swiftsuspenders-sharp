using System;
using System.Reflection;
using System.Collections.Generic;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.errors;

namespace swiftsuspenders.typedescriptions
{
	public class ConstructorInjectionPoint : MethodBaseInjectionPoint
	{
		private ConstructorInfo _constructorInfo;

		public ConstructorInjectionPoint (ConstructorInfo constructorInfo, object[] keys = null, bool optional = false) : base(constructorInfo, keys, optional)
		{
			_constructorInfo = constructorInfo;
		}

		public object CreateInstance(Type type, Injector injector)
		{
			object[] values = GatherParameterValues (type, injector);
			return _constructorInfo.Invoke (values);
		}
	}
}

