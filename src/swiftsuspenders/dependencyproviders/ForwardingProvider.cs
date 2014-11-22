using System;
using System.Collections.Generic;

namespace swiftsuspenders.dependencyproviders
{
	public class ForwardingProvider : DependencyProvider
	{
		public DependencyProvider provider;

		public ForwardingProvider(DependencyProvider provider)
		{
			this.provider = provider;
		}

		public virtual object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			return provider.Apply(targetType, activeInjector, injectParameters);
		}

		public virtual void Destroy ()
		{
			provider.Destroy();
		}
	}
}

