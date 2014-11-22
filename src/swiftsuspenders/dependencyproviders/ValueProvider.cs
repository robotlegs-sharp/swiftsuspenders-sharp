using System;
using System.Collections.Generic;

namespace swiftsuspenders.dependencyproviders
{
	public class ValueProvider : DependencyProvider
	{
		private object _value;
		private Injector _creatingInjector;

		public ValueProvider(object value, Injector creatingInjector = null)
		{
			_value = value;
			_creatingInjector = creatingInjector;
		}

		public object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			return _value;
		}

		public virtual void Destroy ()
		{
			if (_value != null && _creatingInjector != null && _creatingInjector.HasManagedInstance(_value))
			{
				_creatingInjector.DestroyInstance(_value);
			}
			_creatingInjector = null;
			_value = null;
		}
	}
}


