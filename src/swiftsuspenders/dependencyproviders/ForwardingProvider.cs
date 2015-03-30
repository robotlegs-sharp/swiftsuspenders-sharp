using System;
using System.Collections.Generic;

namespace swiftsuspenders.dependencyproviders
{
	public class ForwardingProvider : DependencyProvider
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

		public DependencyProvider provider;
		
		private Action<DependencyProvider, object> _postApply;
		
		private Action<DependencyProvider, object> _preDestroy;

		public ForwardingProvider(DependencyProvider provider)
		{
			this.provider = provider;
		}

		public virtual object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			object instance = provider.Apply(targetType, activeInjector, injectParameters);
			if (_postApply != null)
			{
				_postApply(this, instance);
			}
			return instance;
		}

		public virtual void Destroy ()
		{
			if (_preDestroy != null)
			{
				_preDestroy(this, null);
			}
			provider.Destroy();
		}
	}
}

