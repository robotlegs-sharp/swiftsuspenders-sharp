using System;
using System.Reflection;

namespace swiftsuspenders.typedescriptions
{
	public class ConstructorInjectionPoint : InjectionPoint
	{
		public ConstructorInjectionPoint ()
		{

		}

		public override void ApplyInjection (object target, Type targetType, Injector injector)
		{

		}

		public object CreateInstance(Type type, Injector injector)
		{

			ConstructorInfo[] constructors = type.GetConstructors();
			if (constructors.Length > 0)
				return constructors [0].Invoke (new object[]{ });
			return null;
		}
	}
}

