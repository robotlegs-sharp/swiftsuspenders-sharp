using System;

namespace SwiftSuspenders.Mapping
{
	public struct MappingId
	{
		private Type _type;
		private object _key;

		public Type type 
		{ 
			get 
			{
				return _type;
			}
		}
		public object key 
		{ 
			get 
			{
				return _key;
			}
		}

		public MappingId (Type type, object key)
		{
			_type = type;
			_key = key;
		}

		public MappingId (Type type)
		{
			_type = type;
			_key = null;
		}

		/*
		public override bool Equals (object obj)
		{
			if (obj is MappingId) 
			{
				MappingId other = (MappingId)obj;
				return other.type == type && other.key == key;
			}
			return base.Equals (obj);
		}

		public override int GetHashCode ()
		{
			if (key != null)
				return key.GetHashCode ();
			if (type != null)
				return type.GetHashCode ();
			return 0;

			int hashcode = 0;
			if (type != null)
				hashcode += type.GetHashCode ();
			if (key != null)
				hashcode += key.GetHashCode ();
			return hashcode;
		}
		*/

		public override string ToString ()
		{
			return string.Format ("[MappingId: type={0}, key={1}]", type, key);
		}
	}
}

