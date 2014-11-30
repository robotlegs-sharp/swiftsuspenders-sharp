using System;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.support.types;
using swiftsuspenders;
using System.Collections.Generic;

namespace swiftsuspenders.support.providers
{
	public class MoodyProvider : FallbackDependencyProvider
	{
		private bool _satisfies;

		public const bool ALLOW_INTERFACES = true;

		public MoodyProvider(bool satisfies)
		{
			_satisfies = satisfies;
		}

		public object Apply(Type targetType , Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			return null;
		}

		public void Destroy()
		{
		}

		public bool PrepareNextRequest(object mappingId)
		{
			return _satisfies;
		}
	}
}

