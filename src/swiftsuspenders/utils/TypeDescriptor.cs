using System;
using System.Collections.Generic;
using swiftsuspenders.typedescriptions;
using swiftsuspenders.reflector;

namespace swiftsuspenders.utils
{
	public class TypeDescriptor
	{
		private Dictionary<Type, TypeDescription> _descriptionsCache;
		private Reflector _reflector;

		public TypeDescriptor (Reflector reflector, Dictionary<Type, TypeDescription> descriptionsCache)
		{
			_descriptionsCache = descriptionsCache;
			_reflector = reflector;
		}

		public TypeDescription GetDescription(Type type)
		{
			// get type description or cache it if the given type wasn't encountered before
			TypeDescription description;
			_descriptionsCache.TryGetValue (type, out description);
			if (description == null)
			{
				description = _reflector.DescribeInjections (type);
				_descriptionsCache.Add (type, description);
			}
			return description;
		}

		public void AddDescription(Type type, TypeDescription description)
		{
			_descriptionsCache[type] = description;
		}
	}
}

