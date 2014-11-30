using System;
using swiftsuspenders.typedescriptions;
using System.Collections.Generic;
using System.Reflection;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders;
using swiftsuspenders.errors;
using swiftsuspenders.mapping;

namespace swiftsuspenders.typedescriptions
{
	public abstract class MethodBaseInjectionPoint : InjectionPoint
	{
		protected MethodBase _methodBase;
		protected object[] _keys;
		protected bool _optional;

		public MethodBaseInjectionPoint(MethodBase methodBase, object[] keys = null, bool optional = false)
		{
			_methodBase = methodBase;
			_keys = keys;
			_optional = optional;
		}

		public override void ApplyInjection (object target, Type targetType, Injector injector)
		{
			object[] parameters = GatherParameterValues (targetType, injector);
			_methodBase.Invoke (target, parameters);
		}

		protected virtual object[] GatherParameterValues(Type targetType, Injector injector)
		{
			if (_methodBase == null)
				return new object[0];
			List<object> parameters = new List<object> ();
			ParameterInfo[] parameterInfos = _methodBase.GetParameters();
			int length = parameterInfos.Length;
			for (int i = 0; i < length; i++) 
			{
				Type parameterType = parameterInfos [i].ParameterType;
				MappingId mappingId;
				if (_keys != null && _keys.Length > i && _keys[i] != null)
					mappingId = new MappingId (parameterType, _keys[i]);
				else
					mappingId = new MappingId (parameterType);

				DependencyProvider provider = injector.GetProvider (mappingId);
				if (provider == null) 
				{
					if (parameterInfos [i].IsOptional) 
					{
						parameters.Add (parameterInfos [i].DefaultValue);
						continue;
					}
					if (_optional) 
					{
						parameters.Add (null);
						continue; //TODO: Check optional parameters are in order (last) for this break to work, else use continue
					}
					throw(new InjectorMissingMappingException(
						"Injector is missing a mapping to handle constructor injection into target type '" 
						+ targetType.FullName + "'. \nTarget dependency: " + parameterType.FullName +
						", method: " + _methodBase.Name + ", parameter: " + (i + 1)
					));
				}
				parameters.Add (provider.Apply (targetType, injector, injectParameters));
			}
			return parameters.ToArray();
		}
	}
}

