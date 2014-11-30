using System;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.support.types;
using swiftsuspenders;
using System.Collections.Generic;

namespace swiftsuspenders.support.providers
{
	public class ProviderThatCanDoInterfaces : FallbackDependencyProvider
	{
		private Type _responseType;

		//----------------------               Public Methods               ----------------------//
		public ProviderThatCanDoInterfaces(Type responseType)
		{
			_responseType = responseType;
		}

		public object Apply(Type targetType , Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			return _responseType.GetConstructors()[0].Invoke(null);
		}

		public void Destroy()
		{
		}

		public bool PrepareNextRequest(object mappingId)
		{
			return true;
		}
	}
}

