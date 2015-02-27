using System;
using swiftsuspenders.mapping;
using System.Collections.Generic;

namespace swiftsuspenders.dependencyproviders
{
	public class OtherMappingProvider : DependencyProvider
	{
		public event Action<DependencyProvider, object> PostApply;

		public event Action<DependencyProvider, object> PreDestroy;

		private InjectionMapping _otherMapping;

		public OtherMappingProvider(InjectionMapping mapping)
		{
			_otherMapping = mapping;
		}

		public object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			object instance = _otherMapping.GetProvider ().Apply (targetType, activeInjector, injectParameters);
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

