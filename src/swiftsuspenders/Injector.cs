using System;
using System.Collections.Generic;
using SwiftSuspenders.Mapping;
using SwiftSuspenders.Errors;
using SwiftSuspenders.DependencyProviders;
using SwiftSuspenders.Reflector;
using SwiftSuspenders.Utils;
using SwiftSuspenders.TypeDescriptions;

namespace SwiftSuspenders
{
	public class Injector
	{
		private static readonly Dictionary<Type, bool> BASE_TYPES = new Dictionary<Type, bool> {{typeof(byte), true}, {typeof(sbyte), true}, {typeof(int), true}, {typeof(uint), true}, {typeof(short), true}, {typeof(ushort), true}, {typeof(long), true}, {typeof(ulong), true}, {typeof(float), true}, {typeof(double), true}, {typeof(char), true}, {typeof(bool), true}, {typeof(object), true}, {typeof(string), true}, {typeof(decimal), true}};

		private static Dictionary<Type, TypeDescription> INJECTION_POINTS_CACHE = new Dictionary<Type, TypeDescription>();

		/*============================================================================*/
		/* Events and Delegates                                                       */
		/*============================================================================*/

		public event MappingDelegate PreMappingCreate
		{
			add
			{
				_preMappingCreate += value;
			}
			remove
			{
				_preMappingCreate -= value;
			}
		}

		public event InjectionMappingDelegate PostMappingCreate
		{
			add
			{
				_postMappingCreate += value;
			}
			remove
			{
				_postMappingCreate -= value;
			}
		}

		public event InjectionMappingDelegate PreMappingChange
		{
			add
			{
				_preMappingChange += value;
			}
			remove
			{
				_preMappingChange -= value;
			}
		}

		public event InjectionMappingDelegate PostMappingChange
		{
			add
			{
				_postMappingChange += value;
			}
			remove
			{
				_postMappingChange -= value;
			}
		}

		public event MappingDelegate PostMappingRemove
		{
			add
			{
				_postMappingRemove += value;
			}
			remove
			{
				_postMappingRemove -= value;
			}
		}

		public event InjectionMappingDelegate MappingOverride
		{
			add
			{
				_mappingOverride += value;
			}
			remove
			{
				_mappingOverride -= value;
			}
		}

		public delegate void InjectionMappingDelegate(MappingId mappingId, InjectionMapping instanceType);

		public delegate void MappingDelegate(MappingId mappingId);

		public event InjectionDelegate PostInstantiate
		{
			add
			{
				_postInstantiate += value;
			}
			remove
			{
				_postInstantiate -= value;
			}
		}

		public event InjectionDelegate PreConstruct
		{
			add
			{
				_preConstruct += value;
			}
			remove
			{
				_preConstruct -= value;
			}
		}

		public event InjectionDelegate PostConstruct
		{
			add
			{
				_postConstruct += value;
			}
			remove
			{
				_postConstruct -= value;
			}
		}

		public delegate void InjectionDelegate(object instance, Type instanceType);

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public Injector parentInjector
		{
			get 
			{
				return _parentInjector;
			}
			set 
			{
				_parentInjector = value;
			}
		}

		public FallbackDependencyProvider fallbackProvider
		{
			get 
			{
				return _fallbackProvider;
			}
			set 
			{
				_fallbackProvider = value;
			}
		}

		public bool blockParentFallbackProvider
		{
			get 
			{
				return _blockParentFallbackProvider;
			}
			set 
			{
				_blockParentFallbackProvider = value;
			}
		}

		public Dictionary<MappingId, DependencyProvider> providerMappings = new Dictionary<MappingId, DependencyProvider> ();

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private MappingDelegate _preMappingCreate;
		
		private InjectionMappingDelegate _postMappingCreate;
		
		private InjectionMappingDelegate _preMappingChange;
		
		private InjectionMappingDelegate _postMappingChange;
		
		private MappingDelegate _postMappingRemove;
		
		private InjectionMappingDelegate _mappingOverride;
		
		private InjectionDelegate _postInstantiate;
		
		private InjectionDelegate _preConstruct;
		
		private InjectionDelegate _postConstruct;

		private Injector _parentInjector;

		private TypeDescriptor _typeDescriptor;

		private Dictionary<object, InjectionMapping> _mappings = new Dictionary<object, InjectionMapping>();

		private Dictionary<object, bool> _mappingsInProcess = new Dictionary<object, bool>();

		private Dictionary<object, object> _managedObjects = new Dictionary<object, object>();

		private SwiftSuspenders.Reflector.Reflector _reflector;

		private FallbackDependencyProvider _fallbackProvider;

		private bool _blockParentFallbackProvider = false;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public Injector ()
		{
			_reflector = new SystemReflector();
			_typeDescriptor = new TypeDescriptor(_reflector, INJECTION_POINTS_CACHE);
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public InjectionMapping Map<T>(object key = null)
		{
			return Map (typeof(T), key);
		}

		public InjectionMapping Map(Type type, object key = null)
		{
			MappingId mappingId = new MappingId (type, key);
			return _mappings.ContainsKey (mappingId) ? _mappings [mappingId] : CreateMapping (mappingId); 
		}

		public void Unmap<T>(object key = null)
		{
			Unmap (typeof(T), key);
		}

		public void Unmap(Type type, object key = null)
		{
			MappingId mappingId = new MappingId (type, key);
			InjectionMapping mapping;
			_mappings.TryGetValue(mappingId, out mapping);
			if (mapping != null && mapping.isSealed)
			{
				throw new InjectorException("Can't unmap a sealed mapping");
			}
			if (mapping == null)
			{
				throw new InjectorException("Error while removing an injector mapping: " +
					"No mapping defined for dependency " + mappingId);
			}
			mapping.GetProvider().Destroy();
			_mappings.Remove (mappingId);
			providerMappings.Remove (mappingId);
			if (_postMappingRemove != null)
				_postMappingRemove (mappingId);
		}

		/// <summary>
		/// Indicates whether the injector can supply a response for the specified dependency either
		/// by using a mapping of its own or by querying one of its ancestor injectors.
		/// </summary>
		/// <returns><c>true</c>, if the dependency can be satisfied,, <c>false</c> if not.</returns>
		/// <param name="key">The name of the dependency under query</param>
		/// <typeparam name="T">The type of the dependency under query</typeparam>
		public bool Satisfies<T>(object key = null)
		{
			return Satisfies (typeof(T), key);
		}

		/// <summary>
		/// Indicates whether the injector can supply a response for the specified dependency either
		/// by using a mapping of its own or by querying one of its ancestor injectors.
		/// </summary>
		/// <param name="key">The name of the dependency under query</param>
		/// <typeparam name="T">The type of the dependency under query</typeparam>
		/// <returns><c>true</c>, if the dependency can be satisfied,, <c>false</c> if not.</returns>

		public bool Satisfies(Type type, object key = null)
		{
			MappingId mappingId = new MappingId (type, key);
			return GetProvider(mappingId, true) != null;
		}

		/// <summary>
		/// Indicates whether the injector can directly supply a response for the specified
		/// dependency.
		/// 
		/// <p>In contrast to <code>#satisfies()</code>, <code>satisfiesDirectly</code> only informs
		/// about mappings on this injector itself, without querying its ancestor injectors.</p>
		/// </summary>
		/// <returns><c>true</c>, if the dependency can be satisfied,, <c>false</c> if not.</returns>
		/// <param name="key">The name of the dependency under query</param>
		/// <typeparam name="T">The type of the dependency under query</typeparam>
		public bool SatisfiesDirectly<T>(object key = null)
		{
			return SatisfiesDirectly (typeof(T), key);
		}

		/// <summary>
		/// Indicates whether the injector can directly supply a response for the specified
		/// dependency.
		/// 
		/// <p>In contrast to <code>#satisfies()</code>, <code>satisfiesDirectly</code> only informs
		/// about mappings on this injector itself, without querying its ancestor injectors.</p>
		/// </summary>
		/// <returns><c>true</c>, if the dependency can be satisfied,, <c>false</c> if not.</returns>
		/// <param name="type">The type of the dependency under query</param>
		/// <param name="key">The name of the dependency under query</param>
		public bool SatisfiesDirectly(Type type, object key = null)
		{
			return HasDirectMapping(type, key)
				|| GetDefaultProvider(new MappingId(type,key), false) != null;
		}

		public InjectionMapping GetMapping<T>(object key = null)
		{
			return GetMapping (typeof(T), key);
		}

		public InjectionMapping GetMapping(Type type, object key = null)
		{
			MappingId mappingId = new MappingId (type, key);
			InjectionMapping mapping;
			if (!_mappings.TryGetValue(mappingId, out mapping)) 
			{
				throw new InjectorMissingMappingException("Error while retrieving an injector mapping: "
					+ "No mapping defined for dependency " + mappingId);
			}
			return mapping;
		}

		public bool HasManagedInstance(object instance)
		{
			return _managedObjects.ContainsKey (instance);
		}

		public void InjectInto(object target)
		{
			Type type = target.GetType();
			ApplyInjectionPoints(target, type, _typeDescriptor.GetDescription(type));
		}

		public T GetInstance<T>(object key = null, Type targetType = null)
		{
			return (T)GetInstance (typeof(T), key, targetType);
		}

		public object GetInstance(Type type, object key = null, Type targetType = null)
		{
			MappingId mappingId = new MappingId (type, key);
			DependencyProvider provider = GetProvider (mappingId);
			if (provider == null) provider = GetDefaultProvider(mappingId, true);

			if(provider != null)
			{
//				ConstructorInjectionPoint ctor = _typeDescriptor.GetDescription(type).ctor; //TODO: Make this CTOR
				ConstructorInjectionPoint ctor = null;
				return provider.Apply(targetType, this, ctor != null ? ctor.injectParameters : null);
			}

			string fallbackMessage = _fallbackProvider != null
				? "the fallbackProvider, '" + _fallbackProvider + "', was unable to fulfill this request."
				: "the injector has no fallbackProvider.";

			throw new InjectorMissingMappingException("No mapping found for request " + mappingId
				+ " and " + fallbackMessage);
		}

		public T GetOrCreateNewInstance<T>()
		{
			return (T)GetOrCreateNewInstance (typeof(T));
		}

		public object GetOrCreateNewInstance(Type type)
		{
			return Satisfies (type) ? GetInstance (type) : InstantiateUnmapped (type);
		}

		public T InstantiateUnmapped<T>()
		{
			return (T)InstantiateUnmapped (typeof(T));
		}

		public object InstantiateUnmapped(Type type)
		{
			if(!CanBeInstantiated(type))
			{
				throw new InjectorInterfaceConstructionException(
					"Can't instantiate interface " + type.ToString());
			}
			TypeDescription description = _typeDescriptor.GetDescription(type);
			object instance = description.ctor.CreateInstance(type, this);
			if (_postInstantiate != null)
				_postInstantiate (instance, type);
			ApplyInjectionPoints(instance, type, description);
			return instance;
		}

		public void DestroyInstance(object instance)
		{
			_managedObjects.Remove (instance);
//			const type : Class = _reflector.getClass(instance);
//			Type type = _reflector.GetType(instance);
			Type type = instance.GetType();
			TypeDescription typeDescription = GetTypeDescription(type);
			for (PreDestroyInjectionPoint preDestroyHook = typeDescription.preDestroyMethods;
				preDestroyHook != null; preDestroyHook = preDestroyHook.next as PreDestroyInjectionPoint)
			{
				preDestroyHook.ApplyInjection(instance, type, this);
			}
		}

		public void Teardown()
		{
			foreach (InjectionMapping mapping in _mappings.Values)
			{
				mapping.GetProvider().Destroy();
			}
			List<object> objectsToRemove = new List<object>();
			foreach (KeyValuePair<object,object> managedObject in _managedObjects) 
			{
				objectsToRemove.Add(managedObject.Key);
			}
			while (objectsToRemove.Count > 0) 
			{
				DestroyInstance (objectsToRemove[0]);
				objectsToRemove.RemoveAt (0);
			}
			providerMappings.Clear ();
			_mappings.Clear();
			_mappingsInProcess.Clear();
			_managedObjects.Clear ();
			_fallbackProvider = null;
			_blockParentFallbackProvider = false;
		}

		public Injector CreateChildInjector(/*applicationDomain : ApplicationDomain = null*/)
		{
			Injector injector = new Injector();
//			injector.applicationDomain = applicationDomain || this.applicationDomain;
			injector.parentInjector = this;
			return injector;
		}

		public void AddTypeDescription(Type type, TypeDescription description)
		{
			_typeDescriptor.AddDescription(type, description);
		}

		public TypeDescription GetTypeDescription(Type type)
		{
			return _reflector.DescribeInjections(type);
		}

		public bool HasMapping<T>(object key = null)
		{
			return HasMapping (typeof(T), key);
		}

		public bool HasMapping(Type type, object key = null)
		{
			MappingId mappingId = new MappingId (type, key);
			return GetProvider(mappingId) != null;
		}

		public bool HasDirectMapping<T>(object key = null)
		{
			return HasDirectMapping (typeof(T), key);
		}

		public bool HasDirectMapping(Type type, object key = null)
		{
			MappingId mappingId = new MappingId (type, key);
			return _mappings.ContainsKey(mappingId);
		}

		/*============================================================================*/
		/* Internal Functions                                                         */
		/*============================================================================*/

		public void DispatchPreMappingChangeEvent(MappingId mappingId, InjectionMapping injectionMapping)
		{
			if (_preMappingChange != null)
				_preMappingChange (mappingId, injectionMapping);
		}

		public void DispatchPostMappingChangeEvent(MappingId mappingId, InjectionMapping injectionMapping)
		{
			if (_postMappingChange != null)
				_postMappingChange (mappingId, injectionMapping);
		}

		public void DispatchMappingOverrideEvent(MappingId mappingId, InjectionMapping injectionMapping)
		{
			if (_mappingOverride != null)
				_mappingOverride (mappingId, injectionMapping);
		}

		private bool CanBeInstantiated(Type type)
		{
			TypeDescription description = _typeDescriptor.GetDescription(type);
			return description.ctor != null;
		}

		public DependencyProvider GetProvider(
			MappingId mappingId, bool fallbackToDefault = true)
		{
			DependencyProvider softProvider = null;
			Injector injector = this;
			while (injector != null)
			{
				DependencyProvider provider;
				if (injector.providerMappings.TryGetValue(mappingId, out provider))
				{
					if (provider is SoftDependencyProvider)
					{
						softProvider = provider;
						injector = injector.parentInjector;
						continue;
					}
					if (provider is LocalOnlyProvider && injector != this)
					{
						injector = injector.parentInjector;
						continue;
					}
					return provider;
				}
				injector = injector.parentInjector;
			}
			if (softProvider != null)
			{
				return softProvider;
			}
			return fallbackToDefault ? GetDefaultProvider(mappingId, true) : null;
		}

		private DependencyProvider GetDefaultProvider(
			MappingId mappingId, bool consultParents)
		{
			if (mappingId.key == null && BASE_TYPES.ContainsKey (mappingId.type))
			{
				return null;
			}

			if (_fallbackProvider != null && _fallbackProvider.PrepareNextRequest(mappingId))
			{
				return _fallbackProvider;
			}
			if (consultParents && !_blockParentFallbackProvider && _parentInjector != null)
			{
				return _parentInjector.GetDefaultProvider(mappingId,  consultParents);
			}
			return null;
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private InjectionMapping CreateMapping(MappingId mappingId)
		{
			if (_mappingsInProcess.ContainsKey (mappingId))
				throw new InjectorException ("Can't change a mapping from inside a listener to it's creation event");

			_mappingsInProcess [mappingId] = true;

			if (_preMappingCreate != null)
				_preMappingCreate (mappingId);

			InjectionMapping mapping = new InjectionMapping (this, mappingId);
			_mappings [mappingId] = mapping;

			object sealKey = mapping.Seal ();

			if (_postMappingCreate != null)
				_postMappingCreate (mappingId, mapping);

			_mappingsInProcess.Remove (mappingId);
			mapping.Unseal (sealKey);
			return mapping;
		}

		private void ApplyInjectionPoints(
			object target, Type targetType, TypeDescription description)
		{
			InjectionPoint injectionPoint = description.injectionPoints;
			if (_preConstruct != null)
				_preConstruct (target, targetType);
			while (injectionPoint != null)
			{
				injectionPoint.ApplyInjection(target, targetType, this);
				injectionPoint = injectionPoint.next;
			}
			if (description.preDestroyMethods != null)
			{
				_managedObjects[target] = target;
			}
			if (_postConstruct != null)
				_postConstruct (target, targetType);
		}

		public static void PurgeInjectionPointsCache()
		{
			INJECTION_POINTS_CACHE.Clear ();
		}
	}
}