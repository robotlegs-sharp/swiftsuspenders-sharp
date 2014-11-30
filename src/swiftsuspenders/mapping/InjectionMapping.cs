using System;
using swiftsuspenders.errors;
using swiftsuspenders.dependencyproviders;

namespace swiftsuspenders.mapping
{
	public class InjectionMapping : ProviderlessMapping, UnsealedMapping
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public bool isSealed
		{
			get 
			{
				return _sealed;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Type _type;
		private object _key;
		private object _mappingId;
		private Injector _creatingInjector;
		private bool _defaultProviderSet = false;

		private Injector _overridingInjector = null;
		private bool _soft = false;
		private bool _local = false;
		private bool _sealed;
		private object _sealKey;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/
		public InjectionMapping(Injector injector, Type type, object key, object mappingId)
		{
			_creatingInjector = injector;
			_type = type;
			_key = key;
			_mappingId = mappingId;
			_defaultProviderSet = true;
			MapProvider (new TypeProvider (type));
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public UnsealedMapping AsSingleton(bool initializeImmediately = false)
		{
			ToSingleton(_type, initializeImmediately);
			return this;
		}

		public UnsealedMapping ToType<T>()
		{
			return ToType (typeof(T));
		}

		public UnsealedMapping ToType(Type type)
		{
			ToProvider(new TypeProvider(type));
			return this;
		}

		public UnsealedMapping ToValue(object value, bool autoInject = false, bool destroyOnUnmap = false)
		{
			ToProvider(new ValueProvider(value, destroyOnUnmap ? _creatingInjector : null));
			if (autoInject)
			{
				_creatingInjector.InjectInto(value);
			}
			return this;
		}

		public UnsealedMapping ToSingleton<T>(bool initializeImmediately = false)
		{
			return ToSingleton (typeof(T));
		}

		public UnsealedMapping ToSingleton(Type type, bool initializeImmediately = false)
		{
			ToProvider(new SingletonProvider(type, _creatingInjector));
			if (initializeImmediately) {
				_creatingInjector.GetInstance(_type, _key);
			}
			return this;
		}

		public UnsealedMapping ToProvider(DependencyProvider provider)
		{
			if (_sealed)
				ThrowSealedError();
			if (HasProvider() && provider != null && !_defaultProviderSet)
			{
				Console.WriteLine("Warning: Injector already has a mapping for " + _mappingId + ".\n " +
					"If you have overridden this mapping intentionally you can use " +
					"'injector.unmap()' prior to your replacement mapping in order to " +
					"avoid seeing this message.");
				_creatingInjector.DispatchMappingOverrideEvent (_type, _key, this);
			}
			DispatchPreChangeEvent();
			_defaultProviderSet = false;
			MapProvider(provider);
			DispatchPostChangeEvent();
			return this;
		}

		public UnsealedMapping ToProviderOf(Type type, object key = null)
		{
			DependencyProvider provider = _creatingInjector.GetMapping(type, key).GetProvider();
			ToProvider(provider);
			return this;
		}

		public ProviderlessMapping Softly()
		{
			if (_sealed)
				ThrowSealedError();
			if (!_soft)
			{
				DependencyProvider provider = GetProvider();
				DispatchPreChangeEvent();
				_soft = true;
				MapProvider(provider);
				DispatchPostChangeEvent();
			}
			return this;
		}

		public ProviderlessMapping Locally()
		{
			if (_sealed)
				ThrowSealedError();
			if (_local)
			{
				return this;
			}
			DependencyProvider provider = GetProvider();
			DispatchPreChangeEvent();
			_local = true;
			MapProvider(provider);
			DispatchPostChangeEvent();
			return this;
		}

		public object Seal()
		{
			if (_sealed)
			{
				throw new InjectorException("Mapping is already sealed.");
			}
			_sealed = true;
			_sealKey = new object();
			return _sealKey;
		}

		public InjectionMapping Unseal(object key)
		{
			if (!_sealed)
			{
				throw new InjectorException("Can't unseal a non-sealed mapping.");
			}
			if (key != _sealKey)
			{
				throw new InjectorException("Can't unseal mapping without the correct key.");
			}
			_sealed = false;
			_sealKey = null;
			return this;
		}

		public bool HasProvider()
		{
			return _creatingInjector.providerMappings.ContainsKey(_mappingId);
		}

		public DependencyProvider GetProvider()
		{
//			DependencyProvider provider = _creatingInjector.SsInternal::providerMappings[_mappingId]; TODO: Make this namespace function call
			DependencyProvider provider = _creatingInjector.providerMappings[_mappingId];
			while (provider is ForwardingProvider)
			{
				provider = (provider as ForwardingProvider).provider;
			}
			return provider;
		}

		public InjectionMapping SetInjector(Injector injector)
		{
			if (_sealed)
				ThrowSealedError();

			if (injector == _overridingInjector)
			{
				return this;
			}

			DependencyProvider provider = GetProvider();
			_overridingInjector = injector;
			MapProvider(provider);
			return this;
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void MapProvider(DependencyProvider provider)
		{
			if (_soft)
			{
				provider = new SoftDependencyProvider(provider);
			}
			if (_local)
			{
				provider = new LocalOnlyProvider(provider);
			}
			if (_overridingInjector != null)
			{
				provider = new InjectorUsingProvider(_overridingInjector, provider);
			}
//			_creatingInjector.SsInternal::providerMappings[_mappingId] = provider; //TODO: Make this a namespace function call
			_creatingInjector.providerMappings[_mappingId] = provider;
		}

		private void ThrowSealedError()
		{
			throw new InjectorException("Can't change a sealed mapping");
		}


		private void DispatchPreChangeEvent()
		{
			_creatingInjector.DispatchPreMappingChangeEvent (_type, _key, this);
		}

		private void DispatchPostChangeEvent()
		{
			_creatingInjector.DispatchPostMappingChangeEvent (_type, _key, this);
		}
	}
}

