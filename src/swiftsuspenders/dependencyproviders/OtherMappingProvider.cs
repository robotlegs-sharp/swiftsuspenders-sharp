using System;
using SwiftSuspenders.Mapping;
using System.Collections.Generic;

namespace SwiftSuspenders.DependencyProviders
{
	public class OtherMappingProvider : DependencyProvider
	{
		public event Action<DependencyProvider, object> PostApply
		{
			add
			{
				_postApply += value;
			}
			remove
			{
				_postApply -= value;
			}
		}
		
		public event Action<DependencyProvider, object> PreDestroy
		{
			add
			{
				_preDestroy += value;
			}
			remove
			{
				_preDestroy -= value;
			}
		}
		private Action<DependencyProvider, object> _postApply;
		
		private Action<DependencyProvider, object> _preDestroy;

		private InjectionMapping _otherMapping;

		public OtherMappingProvider(InjectionMapping mapping)
		{
			_otherMapping = mapping;
		}

		public object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			object instance = _otherMapping.GetProvider ().Apply (targetType, activeInjector, injectParameters);
			if (_postApply != null)
			{
				_postApply(this, instance);
			}
			return instance;
		}

		public void Destroy ()
		{
			if (_preDestroy != null) 
			{
				_preDestroy(this, null);
			}
		}
	}
}

