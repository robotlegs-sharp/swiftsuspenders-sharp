﻿using System;
using System.Reflection;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.errors;

namespace swiftsuspenders.typedescriptions
{
	public class FieldInjectionPoint : InjectionPoint
	{
		private object _mappingId;
		private FieldInfo _fieldInfo;
		private bool _optional;

		public FieldInjectionPoint (object mappingId, FieldInfo fieldInfo, bool optional)
		{
			_mappingId = mappingId;
			_fieldInfo = fieldInfo;
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
					_fieldInfo.Name + "' of object '" + target + "' with type '" +
					targetType.FullName +
					"'. Target dependency: '" + _mappingId + "'");
			}
			_fieldInfo.SetValue (target, provider.Apply (_fieldInfo.FieldType, injector, injectParameters));
		}
	}
}
