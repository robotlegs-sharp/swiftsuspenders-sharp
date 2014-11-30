using System;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.support.types;
using swiftsuspenders;
using System.Collections.Generic;

namespace swiftsuspenders.support.providers
{
	public class ClassNameStoringProvider : DependencyProvider
	{
		public string lastTargetClassName;

		public object Apply(Type targetType , Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			lastTargetClassName = targetType.AssemblyQualifiedName;
			return new Clazz();
		}

		public void Destroy()
		{
		}
	}
}

