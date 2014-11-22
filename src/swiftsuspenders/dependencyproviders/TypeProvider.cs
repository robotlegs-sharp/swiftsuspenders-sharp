using System;
using System.Collections.Generic;

namespace swiftsuspenders.dependencyproviders
{
	public class TypeProvider : DependencyProvider
	{
		private Type _responseType;

		public TypeProvider (Type responseType)
		{
			_responseType = responseType;
		}

		public object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			return activeInjector.InstantiateUnmapped(_responseType);
		}

		public void Destroy ()
		{
		
		}
	}
}

