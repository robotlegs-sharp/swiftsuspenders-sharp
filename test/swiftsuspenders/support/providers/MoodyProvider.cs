using System;
using SwiftSuspenders.DependencyProviders;
using SwiftSuspenders.Support.Types;
using SwiftSuspenders;
using System.Collections.Generic;

namespace SwiftSuspenders.Support.providers
{
	public class MoodyProvider : FallbackDependencyProvider
	{
		public event Action<DependencyProvider, object> PostApply;
		public event Action<DependencyProvider, object> PreDestroy;

		private bool _satisfies;

		public const bool ALLOW_INTERFACES = true;

		public MoodyProvider(bool satisfies)
		{
			_satisfies = satisfies;
		}

		public object Apply(Type targetType , Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			if (PostApply != null)
			{
				PostApply(this, null);
			}
			return null;
		}

		public void Destroy()
		{
			if (PreDestroy != null) 
			{
				PreDestroy (this, null);
			}
		}

		public bool PrepareNextRequest(object mappingId)
		{
			return _satisfies;
		}
	}
}

