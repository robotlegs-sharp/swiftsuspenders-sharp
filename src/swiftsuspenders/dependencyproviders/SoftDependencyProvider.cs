using System;

namespace swiftsuspenders.dependencyproviders
{
	public class SoftDependencyProvider : ForwardingProvider
	{
		public SoftDependencyProvider (DependencyProvider provider) : base (provider)
		{
		}
	}
}