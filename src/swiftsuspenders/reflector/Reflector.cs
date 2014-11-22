using System;
using swiftsuspenders.typedescriptions;

namespace swiftsuspenders.reflector
{
	public interface Reflector
	{
//		Type GetClass(Type value);
//		function getFQCN(value : *, replaceColons : Boolean = false) : String;
//		function typeImplements(type : Class, superType : Class) : Boolean;
		TypeDescription DescribeInjections(Type type);
	}
}

