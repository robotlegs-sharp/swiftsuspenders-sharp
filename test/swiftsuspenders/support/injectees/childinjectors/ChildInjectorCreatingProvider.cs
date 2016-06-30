using System;
using SwiftSuspenders.DependencyProviders;

namespace SwiftSuspenders.Support.Injectees.childinjectors
{
	public class ChildInjectorCreatingProvider : DependencyProvider
	{
		public event Action<DependencyProvider, object> PostApply;
		public event Action<DependencyProvider, object> PreDestroy;

		public object Apply (Type targetType, Injector activeInjector, System.Collections.Generic.Dictionary<string, object> injectParameters)
		{
			object instance = activeInjector.CreateChildInjector();
			if (PostApply != null)
			{
				PostApply(this, instance);
			}
			return instance;
		}

		public void Destroy ()
		{
			if (PreDestroy != null) 
			{
				PreDestroy (this, null);
			}
		}
	}
}

