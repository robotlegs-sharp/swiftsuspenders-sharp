using System;
using System.Collections.Generic;
using swiftsuspenders.errors;

namespace swiftsuspenders.dependencyproviders
{
	public class SingletonProvider : DependencyProvider
	{
		public event Action<DependencyProvider, object> PostApply;
		public event Action<DependencyProvider, object> PreDestroy;

		private Type _responseType;
		private Injector _creatingInjector;
		private object _response;
		private bool _destroyed;

		public SingletonProvider(Type responseType, Injector creatingInjector)
		{
			_responseType = responseType;
			_creatingInjector = creatingInjector;
		}

		public object Apply (Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			if (_response == null)
				_response = CreateResponse (_creatingInjector);

			if (PostApply != null)
				PostApply (this, _response);

			return _response;
		}

		public void Destroy ()
		{
			if (PreDestroy != null)
				PreDestroy (this, _response);

			_destroyed = true;
			if (_response != null && _creatingInjector != null && _creatingInjector.HasManagedInstance(_response))
			{
				_creatingInjector.DestroyInstance(_response);
			}
			_creatingInjector = null;
			_response = null;
		}

		private object CreateResponse(Injector injector)
		{
			if (_destroyed)
			{
				throw new InjectorException("Forbidden usage of unmapped singleton provider for type "
					+ _responseType.ToString());
			}
			return injector.InstantiateUnmapped(_responseType);
		}
	}
}


