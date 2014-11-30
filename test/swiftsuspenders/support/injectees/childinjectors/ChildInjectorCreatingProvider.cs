using System;
using swiftsuspenders.dependencyproviders;

namespace swiftsuspenders.support.injectees.childinjectors
{
	public class ChildInjectorCreatingProvider : DependencyProvider
	{
		public object Apply (Type targetType, Injector activeInjector, System.Collections.Generic.Dictionary<string, object> injectParameters)
		{
			return activeInjector.CreateChildInjector();
		}

		public void Destroy ()
		{
			throw new NotImplementedException ();
		}
	}
}

