using System;
using System.Reflection;
using System.Collections.Generic;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.errors;

namespace swiftsuspenders.typedescriptions
{
	public class ConstructorInjectionPoint : InjectionPoint
	{
		private ConstructorInfo _constructorInfo;
		private bool _optional;

		public ConstructorInjectionPoint (ConstructorInfo constructorInfo = null, bool optional = false)
		{
			_constructorInfo = constructorInfo;
			_optional = optional;
		}

		public override void ApplyInjection (object target, Type targetType, Injector injector)
		{

		}

		public object CreateInstance(Type type, Injector injector)
		{
			object[] values = GatherParameterValues (type, injector);
			if (_constructorInfo == null) {
				int length = type.GetConstructors ().Length;
				Console.WriteLine ("FUck");
			}
			return _constructorInfo.Invoke (values);
//			return null;
		}

		protected virtual object[] GatherParameterValues(Type targetType, Injector injector)
		{
			if (_constructorInfo == null)
				return new object[0];
			List<object> parameters = new List<object> ();
			ParameterInfo[] parameterInfos = _constructorInfo.GetParameters();
			int length = parameterInfos.Length;
			for (int i = 0; i < length; i++) 
			{
				Type parameterType = parameterInfos [i].ParameterType;
				DependencyProvider provider = injector.GetProvider (parameterType);
				if (provider == null) 
				{
					if (parameterInfos [i].IsOptional || _optional) 
					{
						parameters [i] = parameterInfos [i].DefaultValue;
						continue; //TODO: Check optional parameters are in order (last) for this break to work, else use continue
					}
					throw(new InjectorMissingMappingException(
						"Injector is missing a mapping to handle constructor injection into target type '" 
						+ targetType.FullName + "'. \nTarget dependency: " + parameterType.FullName +
						", method: " + _constructorInfo.Name + ", parameter: " + (i + 1)
					));
				}
				parameters.Add (provider.Apply (targetType, injector, injectParameters));
//				parameters [i] = provider.Apply (targetType, injector, injectParameters);
			}
			return parameters.ToArray();
		}
	}
}

