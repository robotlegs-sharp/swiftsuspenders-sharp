using System;
using System.Collections.Generic;

namespace swiftsuspenders.dependencyproviders
{
	public class ForwardingProvider : DependencyProvider
	{
		public event Action<DependencyProvider, object> PostApply;
		
		public event Action<DependencyProvider, object> PreDestroy;

		public DependencyProvider provider;

		public ForwardingProvider(DependencyProvider provider)
		{
			this.provider = provider;
		}

		public virtual object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			object instance = provider.Apply(targetType, activeInjector, injectParameters);
			if (PostApply != null)
			{
				PostApply(this, instance);
			}
			return instance;
		}

		public virtual void Destroy ()
		{
			if (PreDestroy != null)
			{
				PreDestroy(this, null);
			}
			provider.Destroy();
		}
	}
}

