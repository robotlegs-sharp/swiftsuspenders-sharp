using System;

namespace SwiftSuspenders.DependencyProviders
{
	public class LocalOnlyProvider : ForwardingProvider
	{
		public LocalOnlyProvider (DependencyProvider provider) : base (provider)
		{
		}
	}
}

