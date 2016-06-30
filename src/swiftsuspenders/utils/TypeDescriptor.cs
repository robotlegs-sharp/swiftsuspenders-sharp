using System;
using System.Collections.Generic;
using SwiftSuspenders.TypeDescriptions;
using SwiftSuspenders.Reflector;

namespace SwiftSuspenders.Utils
{
	public class TypeDescriptor
	{
		private Dictionary<Type, TypeDescription> _descriptionsCache;
		private SwiftSuspenders.Reflector.Reflector _reflector;

		public TypeDescriptor (SwiftSuspenders.Reflector.Reflector reflector, Dictionary<Type, TypeDescription> descriptionsCache)
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

