using System;

namespace swiftsuspenders.dependencyproviders
{
	public interface FallbackDependencyProvider : DependencyProvider
	{
		bool PrepareNextRequest (object mappingId);
	}
}

