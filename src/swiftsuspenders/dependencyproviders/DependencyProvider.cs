using System;
using System.Collections.Generic;

namespace swiftsuspenders.dependencyproviders
{
	public interface DependencyProvider
	{
		object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters);
		void Destroy ();
	}
}

