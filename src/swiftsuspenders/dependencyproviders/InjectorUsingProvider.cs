using System;

namespace SwiftSuspenders.DependencyProviders
{
	public class InjectorUsingProvider : ForwardingProvider
	{
		public Injector injector;

		public InjectorUsingProvider (Injector injector, DependencyProvider provider) : base (provider)
		{
			this.injector = injector;
		}

		public override object Apply (Type targetType, Injector activeInjector, System.Collections.Generic.Dictionary<string, object> injectParameters)
		{
			return provider.Apply(targetType, injector, injectParameters);
		}
	}
}

