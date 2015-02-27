using System;
using System.Collections.Generic;

namespace swiftsuspenders.dependencyproviders
{
	public class TypeProvider : DependencyProvider
	{
		public event Action<DependencyProvider, object> PostApply;
		public event Action<DependencyProvider, object> PreDestroy;

		private Type _responseType;

		public TypeProvider (Type responseType)
		{
			_responseType = responseType;
		}

		public object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			object instance = activeInjector.InstantiateUnmapped(_responseType);
			if (PostApply != null) 
			{
				PostApply(this, instance);
			}
			return instance;
		}

		public void Destroy ()
		{
			if (PreDestroy != null) 
			{
				PreDestroy(this, null);
			}
		}
	}
}

