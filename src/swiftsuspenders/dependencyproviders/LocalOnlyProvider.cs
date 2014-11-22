using System;

namespace swiftsuspenders.dependencyproviders
{
	public class LocalOnlyProvider : ForwardingProvider
	{
		public LocalOnlyProvider (DependencyProvider provider) : base (provider)
		{
		}
	}
}

