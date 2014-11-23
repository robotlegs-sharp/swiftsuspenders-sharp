using System;
using System.Collections.Generic;
using swiftsuspenders.mapping;
using swiftsuspenders.errors;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.reflector;
using swiftsuspenders.utils;
using swiftsuspenders.typedescriptions;

namespace swiftsuspenders
{
	public class Injector
	{
		private static Dictionary<Type, TypeDescription> INJECTION_POINTS_CACHE = new Dictionary<Type, TypeDescription>();

		//TODO: Make basetypes better by making mappingId an class with key and type and then checking the type
//		private static _baseTypes:Array = initBaseTypeMappingIds(
//			[Object, Array, Class, Function, Boolean, Number, int, uint, String]);

//		private static function initBaseTypeMappingIds(types : Array) : Array
//		{
//			return types.map(function(type : Class, index : uint, list : Array) : String
//				{
//					return getQualifiedClassName(type) + '|';
//				});
//		}

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

		public Dictionary<object, DependencyProvider> providerMappings = new Dictionary<object, DependencyProvider> ();

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Injector _parentInjector;

		private TypeDescriptor _typeDescriptor;

		private Dictionary<object, InjectionMapping> _mappings = new Dictionary<object, InjectionMapping>();

		private Dictionary<object, bool> _mappingsInProcess = new Dictionary<object, bool>();

		private Dictionary<object, object> _managedObjects = new Dictionary<object, object>();

		private Reflector _reflector;

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

		public InjectionMapping Map(Type type, Enum key = null)
		{
			object mappingId = key == null ? type as object : key as object;
			return _mappings.ContainsKey (mappingId) ? _mappings [mappingId] : CreateMapping (type, key, mappingId); 
		}

		public void Unmap(Type type, Enum key = null)
		{
			object mappingId = key == null ? type as object : key as object;
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
//			hasEventListener(MappingEvent.POST_MAPPING_REMOVE) && dispatchEvent(
//				new MappingEvent(MappingEvent.POST_MAPPING_REMOVE, type, name, null)); //TODO: Dispatch event
		}

		public bool Satisfies(Type type, Enum key = null)
		{
			object mappingId = key == null ? type as object : key as object;
			return GetProvider(mappingId, true) != null;
		}

		public bool SatisfiesDirectly(Type type, Enum key = null)
		{
			return HasDirectMapping(type, key)
				|| GetDefaultProvider(key, false) != null;
		}

		public InjectionMapping GetMapping(Type type, Enum key = null)
		{
			object mappingId = key == null ? type as object : key as object;
			InjectionMapping mapping;
			if (_mappings.TryGetValue(mappingId, out mapping)) 
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
//			const type : Class = _reflector.getClass(target);
			Type type = target.GetType();
			ApplyInjectionPoints(target, type, _typeDescriptor.GetDescription(type));
		}

		public object GetInstance(Type type, Enum key = null, Type targetType = null)
		{
			object mappingId = key == null ? type as object : key as object;
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

		public object GetOrCreateNewInstance(Type type)
		{
			object instance = null;
			if (Satisfies (type))
				instance = type;
			if (instance == null)
				instance = InstantiateUnmapped (type);
			return instance;
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
//			hasEventListener(InjectionEvent.POST_INSTANTIATE) && dispatchEvent(
//				new InjectionEvent(InjectionEvent.POST_INSTANTIATE, instance, type)); //TODO: Implement events
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
			foreach (object instance in _managedObjects)
			{
				if (instance != null)
					objectsToRemove.Add(instance);
			}
			while(objectsToRemove.Count != 0)
			{
				DestroyInstance(objectsToRemove[0]);
				objectsToRemove.RemoveAt(0);
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

		public bool HasMapping(Type type, Enum key = null)
		{
			object mappingId = key == null ? type as object : key as object;
			return GetProvider(mappingId) != null;
		}

		public bool HasDirectMapping(Type type, Enum key = null)
		{
			object mappingId = key == null ? type as object : key as object;
			return _mappings.ContainsKey(mappingId);
		}

		private bool CanBeInstantiated(Type type)
		{
			TypeDescription description = _typeDescriptor.GetDescription(type);
			return description.ctor != null;
		}

		public DependencyProvider GetProvider(
			object mappingId, bool fallbackToDefault = true)
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
			object mappingId, bool consultParents)
		{
			//No meaningful way to automatically create base types without names
			//TODD: Make the basetypes check work
//			if (_baseTypes.indexOf(mappingId) > -1)
//			{
//				return null;
//			}

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

		private InjectionMapping CreateMapping(Type type, Enum key, object mappingId)
		{
			if (_mappingsInProcess.ContainsKey (mappingId))
				throw new InjectorException ("Can't change a mapping from inside a listener to it's creation event");

			_mappingsInProcess [mappingId] = true;

			//TODO: Dispatch mapping event

			InjectionMapping mapping = new InjectionMapping (this, type, key, mappingId);
			_mappings [mappingId] = mapping;

			object sealKey = mapping.Seal ();

			//TODO: Dispatch mapping event

			_mappingsInProcess.Remove (mappingId);
			mapping.Unseal (sealKey);
			return mapping;
		}

		private void ApplyInjectionPoints(
			object target, Type targetType, TypeDescription description)
		{
			InjectionPoint injectionPoint = description.injectionPoints;
//			hasEventListener(InjectionEvent.PRE_CONSTRUCT) && dispatchEvent(
//				new InjectionEvent(InjectionEvent.PRE_CONSTRUCT, target, targetType)); ?/ TODO: Implement events
			while (injectionPoint != null)
			{
				injectionPoint.ApplyInjection(target, targetType, this);
				injectionPoint = injectionPoint.next;
			}
			if (description.preDestroyMethods != null)
			{
				_managedObjects[target] = target;
			}
//			hasEventListener(InjectionEvent.POST_CONSTRUCT) && dispatchEvent(
//				new InjectionEvent(InjectionEvent.POST_CONSTRUCT, target, targetType)); //TODO@ Implement events
		}
	}
}