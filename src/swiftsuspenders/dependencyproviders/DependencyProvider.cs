using System;
using System.Collections.Generic;

namespace swiftsuspenders.dependencyproviders
{
	public interface DependencyProvider
	{
		event Action<DependencyProvider, object> PostApply;
		event Action<DependencyProvider, object> PreDestroy;

		object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters);
		void Destroy ();
	}
}

