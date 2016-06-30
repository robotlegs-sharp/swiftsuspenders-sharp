using System;

namespace SwiftSuspenders.DependencyProviders
{
	public class SoftDependencyProvider : ForwardingProvider
	{
		public SoftDependencyProvider (DependencyProvider provider) : base (provider)
		{
		}
	}
}