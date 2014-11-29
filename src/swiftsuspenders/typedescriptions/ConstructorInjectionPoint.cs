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
		private object[] _keys;
		private bool _optional;

		public ConstructorInjectionPoint (ConstructorInfo constructorInfo = null, object[] keys = null, bool optional = false)
		{
			_constructorInfo = constructorInfo;
			_keys = keys;
			_optional = optional;
		}

		public override void ApplyInjection (object target, Type targetType, Injector injector)
		{

		}

		public object CreateInstance(Type type, Injector injector)
		{
			object[] values = GatherParameterValues (type, injector, _keys);
			return _constructorInfo.Invoke (values);
		}

		protected virtual object[] GatherParameterValues(Type targetType, Injector injector, object[] keys = null)
		{
			if (_constructorInfo == null)
				return new object[0];
			List<object> parameters = new List<object> ();
			ParameterInfo[] parameterInfos = _constructorInfo.GetParameters();
			int length = parameterInfos.Length;
			for (int i = 0; i < length; i++) 
			{
				Type parameterType = parameterInfos [i].ParameterType;
				object mappingId = parameterType as object;
				if (keys != null && keys.Length > i)
					mappingId = keys[i];
				
				DependencyProvider provider = injector.GetProvider (mappingId);
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

