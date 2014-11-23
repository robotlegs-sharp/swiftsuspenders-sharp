using System;
using System.Reflection;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.errors;
using System.Collections.Generic;

namespace swiftsuspenders.typedescriptions
{
	public class MethodInjectionPoint : InjectionPoint
	{
		private object _mappingId;
		private MethodInfo _methodInfo;
		private bool _optional;

		public MethodInjectionPoint (/*object mappingId, */MethodInfo methodInfo, bool optional)
		{
			// method base?
//			MethodInfo _methodInfo = null;
//			ConstructorInfo _ctor = null;
//			_mappingId = mappingId;
			_methodInfo = methodInfo;
			_optional = optional;
		}

		public override void ApplyInjection (object target, Type targetType, Injector injector)
		{
			object[] parameters = GatherParameterValues (target, targetType, injector);
			_methodInfo.Invoke (target, parameters);
		}

		protected virtual object[] GatherParameterValues(object target, Type targetType, Injector injector)
		{
			List<object> parameters = new List<object> ();
			ParameterInfo[] parameterInfos = _methodInfo.GetParameters();
			int length = parameterInfos.Length;
			for (int i = 0; i < length; i++) 
			{
				Type parameterType = parameterInfos.GetType ();
				DependencyProvider provider = injector.GetProvider (parameterType);
				if (provider == null) 
				{
					if (parameterInfos [i].IsOptional || _optional) 
					{
						parameters [i] = parameterInfos [i].DefaultValue;
						continue; //TODO: Check optional parameters are in order (last) for this break to work, else use continue
					}
					throw(new InjectorMissingMappingException(
						"Injector is missing a mapping to handle injection into target '" +
						target + "' of type '" + targetType.FullName + 
						"'. \nTarget dependency: " + parameterType.FullName +
						", method: " + _methodInfo.Name + ", parameter: " + (i + 1)
					));
				}
				parameters [i] = provider.Apply (targetType, injector, injectParameters);
			}
			return parameters.ToArray();
		}
	}
}

