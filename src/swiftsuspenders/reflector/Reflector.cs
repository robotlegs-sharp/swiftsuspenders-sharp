using System;
using SwiftSuspenders.TypeDescriptions;

namespace SwiftSuspenders.Reflector
{
	public interface Reflector
	{
//		Type GetClass(Type value);
//		function getFQCN(value : *, replaceColons : Boolean = false) : String;
//		function typeImplements(type : Class, superType : Class) : Boolean;
		TypeDescription DescribeInjections(Type type);
	}
}

