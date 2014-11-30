using System;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.support.types;
using swiftsuspenders;
using System.Collections.Generic;

namespace swiftsuspenders.support.providers
{
	public class UnknownParametersUsingProvider : DependencyProvider
	{
		public string parameterValue;

		public object Apply(Type targetType , Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			parameterValue = injectParameters["param"] as string;
			return new Clazz();
		}

		public void Destroy()
		{
		}
	}
}

