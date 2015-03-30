using System;
using System.Collections.Generic;

namespace swiftsuspenders.dependencyproviders
{
	public class ValueProvider : DependencyProvider
	{
		public event Action<DependencyProvider, object> PostApply
		{
			add
			{
				_postApply += value;
			}
			remove
			{
				_postApply -= value;
			}
		}
		
		public event Action<DependencyProvider, object> PreDestroy
		{
			add
			{
				_preDestroy += value;
			}
			remove
			{
				_preDestroy -= value;
			}
		}
		
		private Action<DependencyProvider, object> _postApply;
		
		private Action<DependencyProvider, object> _preDestroy;

		private object _value;
		private Injector _creatingInjector;

		public ValueProvider(object value, Injector creatingInjector = null)
		{
			_value = value;
			_creatingInjector = creatingInjector;
		}

		public object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			if (_postApply != null) 
			{
				_postApply(this, _value);
			}
			return _value;
		}

		public virtual void Destroy ()
		{
			if (_preDestroy != null) 
			{
				_preDestroy(this, _value);
			}
			if (_value != null && _creatingInjector != null && _creatingInjector.HasManagedInstance(_value))
			{
				_creatingInjector.DestroyInstance(_value);
			}
			_creatingInjector = null;
			_value = null;
		}
	}
}


