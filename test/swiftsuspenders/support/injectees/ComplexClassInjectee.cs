using System;
using swiftsuspenders.support.types;

namespace swiftsuspenders.support.injectees
{
	public class ComplexClassInjectee
	{
		[Inject]
		public ComplexClazz property { get; set; }
	}
}

