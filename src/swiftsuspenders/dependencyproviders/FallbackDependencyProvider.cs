using System;

namespace SwiftSuspenders.DependencyProviders
{
	public interface FallbackDependencyProvider : DependencyProvider
	{
		bool PrepareNextRequest (object mappingId);
	}
}

