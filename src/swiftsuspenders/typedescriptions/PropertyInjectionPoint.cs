using System;
using swiftsuspenders.typedescriptions;
using System.Reflection;
using swiftsuspenders;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.errors;

namespace swiftsuspenders.typedescriptions
{
	public class PropertyInjectionPoint : InjectionPoint
	{
		private object _mappingId;
		private PropertyInfo _propertyInfo;
		private bool _optional;

		public PropertyInjectionPoint (object mappingId, PropertyInfo propertyInfo, bool optional)
		{
			_mappingId = mappingId;
			_propertyInfo = propertyInfo;
			_optional = optional;
		}

		public override void ApplyInjection (object target, Type targetType, Injector injector)
		{
			DependencyProvider provider = injector.GetProvider (_mappingId);
			if (provider == null) 
			{
				if (_optional)
					return;
				throw new InjectorMissingMappingException("Injector is missing a mapping to handle injection into property '" +
					_propertyInfo.Name + "' of object '" + target + "' with type '" +
					targetType.FullName +
					"'. Target dependency: '" + _mappingId + "'");
			}
			_propertyInfo.SetValue (target, provider.Apply (targetType, injector, injectParameters), null); //TODO: Research indexed properties
		}
	}
}

