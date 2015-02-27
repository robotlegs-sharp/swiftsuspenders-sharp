using System;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.support.types;
using swiftsuspenders;
using System.Collections.Generic;

namespace swiftsuspenders.support.providers
{
	public class ProviderThatCanDoInterfaces : FallbackDependencyProvider
	{
		public event Action<DependencyProvider, object> PostApply;
		public event Action<DependencyProvider, object> PreDestroy;

		private Type _responseType;

		//----------------------               Public Methods               ----------------------//
		public ProviderThatCanDoInterfaces(Type responseType)
		{
			_responseType = responseType;
		}

		public object Apply(Type targetType , Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			object instance = _responseType.GetConstructors()[0].Invoke(null);
			if (PostApply != null) 
			{
				PostApply (this, instance);
			}
			return instance;
		}

		public void Destroy()
		{
			if (PreDestroy != null) 
			{
				PreDestroy (this, null);
			}
		}

		public bool PrepareNextRequest(object mappingId)
		{
			return true;
		}
	}
}

