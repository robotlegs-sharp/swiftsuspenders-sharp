using System;
using swiftsuspenders.mapping;
using System.Collections.Generic;

namespace swiftsuspenders.dependencyproviders
{
	public class OtherMappingProvider : DependencyProvider
	{
		private InjectionMapping _otherMapping;

		public OtherMappingProvider(InjectionMapping mapping)
		{
			_otherMapping = mapping;
		}

		public object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			return _otherMapping.GetProvider ().Apply (targetType, activeInjector, injectParameters);
		}

		public void Destroy ()
		{

		}
	}
}

